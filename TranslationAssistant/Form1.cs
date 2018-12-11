using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Globalization;
using System.Security;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.Win32;

namespace TranslationAssistant
{
	public partial class Form1 : Form
	{
		private string[] topLevelDomainList = null;
		private int[] lineOffsetList = null;
		private decimal lastNumericUpDownLineNumberValue;
		private int clickedLineNumber = 0;

		public Form1()
		{
			InitializeComponent();

			lastNumericUpDownLineNumberValue = numericUpDownLineNumber.Value;
			
			buttonBack.Enabled = false;
			buttonForward.Enabled = false;

			// 디자이너에서 등록할 수 없는 이벤트 등록
			webBrowser.CanGoBackChanged += new EventHandler(webBrowser_CanGoBackChanged);
			webBrowser.CanGoForwardChanged += new EventHandler(webBrowser_CanGoForwardChanged);

			// 웹 브라우저 버전이 설정이 되어있으면 버전 설정이 아니라 설정 제거라고 표시한다.
			if (Form1.WebBrowserVersionSet())
			{
				buttonWebBrowserVersion.Text = "설정 제거";
			}

			// 프로그램을 시작할 때 한 번만 최신 top level domain 목록을 비동기로 로드한다.
			LoadTopLevelDomainList();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// 디자이너에서 AutoWordSelection을 false로 설정해 놓아도 런타임에는 true인 것처럼 동작하는 버그가 있는 것으로 보인다.
			richTextBox.AutoWordSelection = false;
		}

		private void richTextBox_TextChanged(object sender, EventArgs e)
		{
			RichTextBox richTextBox = (RichTextBox)sender;

			// richTextBox에 아무것도 입력되어있지 않은 경우
			if (richTextBox.Lines.Length < numericUpDownLineNumber.Minimum)
			{
				numericUpDownLineNumber.Maximum = numericUpDownLineNumber.Minimum;
				lineOffsetList = null;
				return;
			}

			numericUpDownLineNumber.Maximum = richTextBox.Lines.Length;

			string[] lines = richTextBox.Lines; // RichTextBox의 Lines는 string 배열 객체가 항상 존재하고 있는게 아니라, 호출될 때마다 매번 Text에 Split()을 호출해 임시로 string 배열을 만드는 것으로 보인다. 따라서 아래와 같은 코드에서 Lines를 직접 사용하면 매우 느려진다.

			// RichTextBox의 GetFirstCharIndexFromLine()은 WordWrap이 true인 경우 개행 문자가 아니라 표시된 줄을 기준으로 인덱스를 반환한다. 따라서 직접 각 라인의 인덱스를 미리 저장해 놓아야 한다.
			lineOffsetList = new int[richTextBox.Lines.Length];
			lineOffsetList[0] = 0;
            for (int i = 1; i < lineOffsetList.Length; i++)
			{
				lineOffsetList[i] = lineOffsetList[i - 1] + lines[i - 1].Length + 1;
			}

			TranslateCurrentOrNextLine();
		}

		private void contextMenuStripRichTextBox_Opening(object sender, CancelEventArgs e)
		{
			// 선택된 내용이 있을 때에만 사전에서 찾기 메뉴와 번역 메뉴와 검색 메뉴를 활성화한다.
			if (richTextBox.SelectedText != "")
			{
				dictionaryToolStripMenuItem.Enabled = true;
				translateToolStripMenuItem.Enabled = true;
				searchToolStripMenuItem.Enabled = true;
			}
			else
			{
				dictionaryToolStripMenuItem.Enabled = false;
				translateToolStripMenuItem.Enabled = false;
				searchToolStripMenuItem.Enabled = false;
			}

			// richTextBox에 입력된 내용이 있을 때에만 지정한 라인으로 이동하는 메뉴와 라인을 사전에서 찾는 메뉴와 라인을 번역하는 메뉴를 활성화한다.
			if (lineOffsetList != null)
			{
				dictionaryLineToolStripMenuItem.Enabled = true;
				translateLineToolStripMenuItem.Enabled = true;

				// 마우스가 어느 라인 위에 있는지 확인한다.
				int charIndex = richTextBox.GetCharIndexFromPosition(richTextBox.PointToClient(Cursor.Position));

				clickedLineNumber = Array.BinarySearch(lineOffsetList, charIndex);
				if (clickedLineNumber < 0)
				{
					clickedLineNumber = ~clickedLineNumber;
				}
				else
				{
					clickedLineNumber++;
				}

				// 클릭한 라인이 현재 라인이 아닌 경우에만 지정한 라인으로 이동하는 메뉴를 활성화한다.
				if (clickedLineNumber != numericUpDownLineNumber.Value)
				{
					jumpToolStripMenuItem.Enabled = true;
				}
				else
				{
					jumpToolStripMenuItem.Enabled = false;
				}

				jumpToolStripMenuItem.Text = "이 줄(" + clickedLineNumber + ")로 이동";
			}
			else
			{
				jumpToolStripMenuItem.Enabled = false;
				dictionaryLineToolStripMenuItem.Enabled = false;
				translateLineToolStripMenuItem.Enabled = false;

				jumpToolStripMenuItem.Text = "이 줄로 이동";
			}
		}

		private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			numericUpDownLineNumber.Value = clickedLineNumber;
		}

		private void dictionaryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 선택된 내용을 네이버 일본어 사전으로 검색
			try
			{
				webBrowser.Navigate("https://ja.dict.naver.com/search.nhn?range=all&q=" + Uri.EscapeDataString(richTextBox.SelectedText) + "&sm=jpd_hty");
			}
			catch
			{
			}
		}

		private void translateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 선택된 내용을 구글 번역기로 번역
			try
			{
				webBrowser.Navigate("https://translate.google.co.kr/?hl=ko#ja/ko/" + Uri.EscapeDataString(richTextBox.SelectedText));
			}
			catch
			{
			}
		}

		private void searchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 선택된 내용을 구글 일본어 검색으로 검색
			try
			{
				webBrowser.Navigate("https://www.google.co.jp/#q=" + Uri.EscapeDataString(richTextBox.SelectedText));
			}
			catch
			{
			}
		}

		private void dictionaryLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 오른쪽 클릭한 라인을 네이버 일본어 사전으로 검색
			try
			{
				webBrowser.Navigate("https://ja.dict.naver.com/search.nhn?range=all&q=" + Uri.EscapeDataString(richTextBox.Lines[clickedLineNumber - 1]) + "&sm=jpd_hty");
			}
			catch
			{
			}
		}

		private void translateLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 오른쪽 클릭한 라인을 구글 번역기로 번역
			try
			{
				webBrowser.Navigate("https://translate.google.co.kr/?hl=ko#ja/ko/" + Uri.EscapeDataString(richTextBox.Lines[clickedLineNumber - 1]));
			}
			catch
			{
			}
		}

		private void numericUpDownLineNumber_ValueChanged(object sender, EventArgs e)
		{
			NumericUpDown numericUpDownLineNumber = (NumericUpDown)sender;

			if (numericUpDownLineNumber.Value > lastNumericUpDownLineNumberValue)
			{
				TranslateCurrentOrNextLine();
			}
			else if (numericUpDownLineNumber.Value < lastNumericUpDownLineNumberValue)
			{
				TranslateCurrentOrPreviousLine();
			}
			else  // numericUpDownLineNumber의 버튼을 눌렀지만 이전 라인 또는 다음 라인 중에 내용이 있는 라인이 없으면 위 로직에 의해 numericUpDownLineNumber의 값이 원래대로 바뀐다.
			{
				return;
			}

			lastNumericUpDownLineNumberValue = numericUpDownLineNumber.Value;
		}

		private void buttonPrevious_Click(object sender, EventArgs e)
		{
			TranslatePreviousLine();
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			TranslateNextLine();
		}

		private void checkBoxClipboard_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBoxClipboard = (CheckBox)sender;

			// checkBoxClipboard가 체크되면 1초에 한 번씩 richTextBox의 내용을 클립보드의 내용으로 덮어씌운다. 클립보드의 내용을 주기적으로 가져오는 방법이 아니라 좀 더 어려운 방법을 사용하면, 클립보드의 내용이 바뀌는 것을 직접 감지할 수 있는 듯 하다.
			if (checkBoxClipboard.Checked)
			{
				timerClipboardObserver.Start();

				// 타이머는 1초 후부터 동작하므로, 체크가 됐을 때 일단 한 번 클립보드의 내용을 가져온다.
				LoadClipboardText();
			}
			else
			{
				timerClipboardObserver.Stop();
			}
		}

		private void timerClipboardObserver_Tick(object sender, EventArgs e)
		{
			LoadClipboardText();
		}

		private void buttonBack_Click(object sender, EventArgs e)
		{
			webBrowser.GoBack();
		}

		private void buttonForward_Click(object sender, EventArgs e)
		{
			webBrowser.GoForward();
		}

		private void buttonRefresh_Click(object sender, EventArgs e)
		{
			webBrowser.Refresh();
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			webBrowser.Stop();
		}

		private void buttonTranslate_Click(object sender, EventArgs e)
		{
			// 현재 페이지를 구글 번역기로 번역
			try
			{
				webBrowser.Navigate("https://translate.google.co.kr/translate?hl=ko&sl=ja&tl=ko&u=" + Uri.EscapeDataString(webBrowser.Url.ToString()));
			}
			catch
			{
			}
		}

		private void buttonWebBrowserVersion_Click(object sender, EventArgs e)
		{
			if (Form1.WebBrowserVersionSet())
			{
				DialogResult dialogResult = MessageBox.Show("내장 웹 브라우저 버전 설정을 제거 하시겠습니까?\n설정은 레지스트리의 아래 위치에 이 실행 파일과 같은 이름으로 만들어져 있습니다.\n\nHKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\n\\Internet Explorer\\Main\\FeatureControl\n\\FEATURE_BROWSER_EMULATION\n\n또는\n\nHKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\n\\Internet Explorer\\Main\\FeatureControl\n\\FEATURE_BROWSER_EMULATION\n\n제거한 후에 다시 생성할 수 있습니다. 관리자 권한이 필요합니다.\n\n제거한 후에 프로그램이 자동으로 다시 시작됩니다.", "설정 제거", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (dialogResult != DialogResult.Yes)
				{
					return;
				}

				RegistryKey registryKey = OpenWritableFeatureBrowserEmulationRegistryKey();

				if (registryKey == null)
				{
					return;
				}

				try
				{
					registryKey.DeleteValue(Process.GetCurrentProcess().ProcessName + ".exe");
				}
				catch (SecurityException)
				{
					MessageBox.Show("관리자 권한이 필요합니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				catch (ObjectDisposedException)
				{
					MessageBox.Show("레지스트리 키가 열려있지 않습니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				catch (UnauthorizedAccessException)
				{
					MessageBox.Show("레지스트리 키의 값을 지울 수 없습니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				finally
				{
					registryKey.Close();
				}
			}
			else
			{
				DialogResult dialogResult = MessageBox.Show("내장 웹 브라우저 버전을 이 컴퓨터에 설치된 Internet Explorer에 맞추도록 설정 하시겠습니까?\n설정하면 올바로 보이지 않던 페이지가 올바로 보일 수 있습니다.\n예를 선택하면 레지스트리의 아래 위치에 이 실행 파일과 같은 이름의 값을 생성합니다.\n\nHKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\n\\Internet Explorer\\Main\\FeatureControl\n\\FEATURE_BROWSER_EMULATION\n\n또는\n\nHKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\n\\Internet Explorer\\Main\\FeatureControl\n\\FEATURE_BROWSER_EMULATION\n\n생성한 후에 다시 제거할 수 있습니다. 관리자 권한이 필요합니다.\n\n생성한 후에 프로그램이 자동으로 다시 시작됩니다.", "버전 설정", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (dialogResult != DialogResult.Yes)
				{
					return;
				}

				RegistryKey registryKey = OpenWritableFeatureBrowserEmulationRegistryKey();

				if (registryKey == null)
				{
					return;
				}

				int value;

				switch (webBrowser.Version.Major)
				{
					case 11:
						value = 11001;
						break;

					case 10:
						value = 10001;
						break;

					case 9:
						value = 9999;
						break;

					case 8:
						value = 8888;
						break;

					default:
						value = 7000;
						break;
				}

				try
				{
					registryKey.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", value);
				}
				catch (SecurityException)
				{
					MessageBox.Show("관리자 권한이 필요합니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				catch (ObjectDisposedException)
				{
					MessageBox.Show("레지스트리 키가 열려있지 않습니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				catch (UnauthorizedAccessException)
				{
					MessageBox.Show("레지스트리 키에 값을 쓸 수 없습니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				finally
				{
					registryKey.Close();
				}
			}

			Application.Restart();
		}

		private void textBoxAddress_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBoxAddress = (TextBox)sender;

			if (e.KeyCode == Keys.Enter)
			{
				string address = textBoxAddress.Text.Trim();

				// 아무것도 입력되지 않은 경우
				if (String.IsNullOrEmpty(address))
				{
					return;
				}

				try
				{
					// 온전한 URI 형태로 입력되었다면 성공한다.
					webBrowser.Navigate(new Uri(address));
				}
				catch (UriFormatException)
				{
					try
					{
						// 맨 첫 글자가 ?인 경우에는 그 뒤의 단어를 검색어로 간주하고 구글 일본어 검색을 한다.
						if (address.StartsWith("?"))
						{
							webBrowser.Navigate("https://www.google.co.jp/#q=" + Uri.EscapeDataString(address.Substring(1).TrimStart()));
							return;
						}

						// 입력된 문자열을 프로토콜이 HTTP인 URI라고 간주하고 도메인 부분만 분리한다.
						Uri uri = new Uri("http://" + address);
						string[] domain = uri.Host.Split('.');

						// 도메인의 맨 마지막이 top level domain 중 하나라면, 입력된 문자열을 프로토콜이 HTTP인 URI로 간주한다. top level domain은 언제라도 추가될 수 있기 때문에 직접 확인한다.
						if (topLevelDomainList != null)
						{
							// 국제화 도메인일 수도 있으므로 도메인의 맨 마지막을 퓨니코드로 인코딩한다.
							string punycode = new IdnMapping().GetAscii(domain[domain.Length - 1]);

							for (int i = 0; i < topLevelDomainList.Length; i++)
							{
								// top level domain 목록의 맨 첫 줄은 무시한다.
								if (topLevelDomainList[i].TrimStart().StartsWith("#"))
								{
									continue;
								}

								if (String.Compare(topLevelDomainList[i], punycode, true) == 0)
								{
									webBrowser.Navigate(uri);
									return;
								}
							}
						}

						// top level domain 목록을 로드하는데 실패했다면 입력된 문자열을 DNS에 질의해 도메인이 존재하는지에 대한 여부를 확인해야 한다. 그리고 top level domain 목록을 로드하는데 성공한 경우, 일반적인 환경에서는 여기까지 왔다면 입력된 문자열이 URI인 경우가 거의 없겠지만, 만약 자체적으로 네임 서버와 도메인을 만들어서 사용하는 환경이라면 URI일 수도 있으므로 DNS 질의는 무조건 할 수밖에 없다. 하지만 입력된 문자열이 URI가 아니라 검색어인 경우, DNS 질의 결과가 온 후에 검색을 진행하면 검색 결과가 나올 때까지 오래 기다리게 된다. 따라서 DNS 질의는 비동기로 하고 질의 결과가 오기 전에 구글 일본어 검색도 동시에 진행한다.
						TryAsURI(uri);
						try
						{
							webBrowser.Navigate("https://www.google.co.jp/#q=" + Uri.EscapeDataString(address));
						}
						catch
						{
						}
					}
					catch
					{
						// 입력된 문자열을 프로토콜이 HTTP인 URI로 만드는데 실패했다거나, 빈 문자열을 퓨니코드로 인코딩하려고 시도하다 예외가 발생했다거나 하는 등의 경우에는, 입력된 문자열을 검색어로 간주하고 구글 일본어 검색을 한다.
						try
						{
							webBrowser.Navigate("https://www.google.co.jp/#q=" + Uri.EscapeDataString(address));
						}
						catch
						{
						}
					}
				}
				catch
				{
				}
			}
		}

		private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			WebBrowser webBrowser = (WebBrowser)sender;

			textBoxAddress.Text = webBrowser.Url.ToString();
		}

		private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			WebBrowser webBrowser = (WebBrowser)sender;

			// 네이버 일본어 사전 검색인 경우에는 번역 결과가 보이도록 무조건 페이지 맨 아래로 스크롤하고, 엔터만 누르면 다음 문장이 검색될 수 있도록 buttonNext에 포커스를 설정한다.
			if (webBrowser.Url.ToString().StartsWith("https://ja.dict.naver.com/search.nhn?range=all&q=") && webBrowser.Url.ToString().EndsWith("&sm=jpd_hty"))
			{
				webBrowser.Document.Body.ScrollIntoView(false);
				buttonNext.Focus();
			}
		}

		private void webBrowser_CanGoBackChanged(object sender, EventArgs e)
		{
			WebBrowser webBrowser = (WebBrowser)sender;

			buttonBack.Enabled = webBrowser.CanGoBack;
		}

		private void webBrowser_CanGoForwardChanged(object sender, EventArgs e)
		{
			WebBrowser webBrowser = (WebBrowser)sender;

			buttonForward.Enabled = webBrowser.CanGoForward;
		}

		// 현재 라인을 네이버 일본어 사전으로 검색하고, 현재 라인을 표시한다. 이 함수는 직접 사용하지 않고 TranslatePreviousLine(), TranslateCurrentOrNextLine(), TranslateCurrentOrPreviousLine(), TranslateNextLine()을 통해 사용한다.
		private void TranslateLine()
		{
			int lineNumber = (int)numericUpDownLineNumber.Value;

			try
			{
				webBrowser.Navigate("https://ja.dict.naver.com/search.nhn?range=all&q=" + Uri.EscapeDataString(richTextBox.Lines[lineNumber - 1]) + "&sm=jpd_hty");
			}
			catch
			{
			}

			richTextBox.Select(lineOffsetList[lineNumber - 1], richTextBox.Lines[lineNumber - 1].Length);
		}

		// 이전 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 네이버 일본어 사전으로 검색한다. 만약 이전 라인 중에 내용이 있는 라인이 없으면 false를 반환하고, 그렇지 않으면 true를 반환한다.
		private bool TranslatePreviousLine()
		{
			int lineNumber = (int)numericUpDownLineNumber.Value;

			do
			{
				if (--lineNumber < numericUpDownLineNumber.Minimum)
				{
					return false;
				}
			}
			while (String.IsNullOrWhiteSpace(richTextBox.Lines[lineNumber - 1]));

			numericUpDownLineNumber.Value = lineNumber;

			return true;
		}

		// 현재 라인을 네이버 일본어 사전으로 검색한다. 현재 라인에 내용이 없는 경우 다음 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 검색하고, 다음 라인 중에 내용이 있는 라인이 없으면 이전 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 검색한다.
		private void TranslateCurrentOrNextLine()
		{
			int lineNumber = (int)numericUpDownLineNumber.Value;

			if (richTextBox.Lines.Length < numericUpDownLineNumber.Minimum)
			{
				return;
			}

			if (String.IsNullOrWhiteSpace(richTextBox.Lines[lineNumber - 1]))
			{
				if (!TranslateNextLine())
				{
					TranslatePreviousLine();
				}
			}
			else
			{
				TranslateLine();
			}
		}

		// 현재 라인을 네이버 일본어 사전으로 검색한다. 현재 라인에 내용이 없는 경우 이전 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 검색하고, 이전 라인 중에 내용이 있는 라인이 없으면 다음 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 검색한다.
		private void TranslateCurrentOrPreviousLine()
		{
			int lineNumber = (int)numericUpDownLineNumber.Value;

			if (richTextBox.Lines.Length < numericUpDownLineNumber.Minimum)
			{
				return;
			}

			if (String.IsNullOrWhiteSpace(richTextBox.Lines[lineNumber - 1]))
			{
				if (!TranslatePreviousLine())
				{
					TranslateNextLine();
				}
			}
			else
			{
				TranslateLine();
			}
		}

		// 다음 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 네이버 일본어 사전으로 검색한다. 만약 다음 라인 중에 내용이 있는 라인이 없으면 false를 반환하고, 그렇지 않으면 true를 반환한다.
		private bool TranslateNextLine()
		{
			int lineNumber = (int)numericUpDownLineNumber.Value;

			do
			{
				if (++lineNumber > numericUpDownLineNumber.Maximum)
				{
					return false;
				}
			}
			while (String.IsNullOrWhiteSpace(richTextBox.Lines[lineNumber - 1]));

			numericUpDownLineNumber.Value = lineNumber;

			return true;
		}

		// 클립보드의 내용이 텍스트이고 richTextBox와 내용이 다르면, richTextBox의 내용을 클립보드의 내용으로 덮어씌운다.
		private void LoadClipboardText()
		{
			if (!Clipboard.ContainsText())
			{
				return;
			}

			string clipboard = Clipboard.GetText().Replace("\r\n", "\n");

			if (richTextBox.Text != clipboard)
			{
				richTextBox.Text = clipboard;
			}
		}

		// 최신 top level domain 목록을 비동기로 로드한 다음, MD5 검사를 하고, 목록을 배열로 만들어 저장한다.
		private async void LoadTopLevelDomainList()
		{
			HttpClient client = new HttpClient();
			string topLevelDomainsContent;
			string[] topLevelDomainsMD5;
			StringBuilder stringBuilder;

			do
			{
				Task<string> topLevelDomainsTask = client.GetStringAsync("https://data.iana.org/TLD/tlds-alpha-by-domain.txt");
				Task<string> topLevelDomainsMD5Task = client.GetStringAsync("https://data.iana.org/TLD/tlds-alpha-by-domain.txt.md5");

				try
				{
					topLevelDomainsContent = await topLevelDomainsTask;
					topLevelDomainsMD5 = (await topLevelDomainsMD5Task).Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
				}
				catch
				{
					return;
				}

				MD5 md5 = MD5.Create();
				byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(topLevelDomainsContent));

				stringBuilder = new StringBuilder();
				for (int i = 0; i < hash.Length; i++)
				{
					stringBuilder.Append(hash[i].ToString("x2"));
				}
			}
			while (stringBuilder.ToString() != topLevelDomainsMD5[0] || topLevelDomainsMD5[1] != "tlds-alpha-by-domain.txt");

			topLevelDomainList = topLevelDomainsContent.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		}

		// URI의 도메인 부분을 DNS에 질의해 도메인이 존재하는지에 대한 여부를 비동기로 확인한 후 존재하면 해당 웹 페이지를 로드한다.
		private async void TryAsURI(Uri uri)
		{
			try
			{
				Task<IPAddress[]> hostAddressesTask = Dns.GetHostAddressesAsync(uri.Host);
				IPAddress[] hostAddresses = await hostAddressesTask;

				webBrowser.Navigate(uri);
			}
			catch
			{
			}
		}

		// FEATURE_BROWSER_EMULATION 레지스트리 키를 연다. 실패하면 에러 메시지 박스를 보여주고 null을 반환한다.
		private RegistryKey OpenWritableFeatureBrowserEmulationRegistryKey()
		{
			RegistryKey registryKey;

			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
			}
			catch (SecurityException)
			{
				MessageBox.Show("관리자 권한이 필요합니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
			catch (ObjectDisposedException)
			{
				MessageBox.Show("레지스트리 키가 열려있지 않습니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}

			if (registryKey == null)
			{
				MessageBox.Show("레지스트리의 해당 위치에 키가 존재하지 않습니다.", buttonWebBrowserVersion.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return registryKey;
		}

		// 웹 브라우저 버전이 설정이 되어있으면 true를 반환하고 되어있지 않거나 되어있는지 알 수 없으면 false를 반환한다.
		private static bool WebBrowserVersionSet()
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION");

				if (registryKey != null)
				{
					try
					{
						object value = registryKey.GetValue(Process.GetCurrentProcess().ProcessName + ".exe");

						if (value != null)
						{
							return true;
						}
					}
					finally
					{
						registryKey.Close();
					}
				}
			}
			catch
			{
			}

			return false;
		}
	}
}
