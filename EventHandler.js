const { clipboard } = require('electron');
const punycode = require('punycode');
const tlds_alpha_by_domain = require('./tlds-alpha-by-domain.js');

var clipboardText;
var interval;
var topLevelDomainList = null;
var lineOffsetList = null;
var lastNumericUpDownLineNumberValue;

function OnBodyLoad() {
	lastNumericUpDownLineNumberValue = document.getElementById("numericUpDownLineNumber").value;
	
	document.getElementById("textAreaOriginalText").addEventListener("mouseup", OnTextAreaOriginalTextMouseUp);
	document.getElementById("textBoxAddress").addEventListener("keydown", OnTextBoxAddressKeyDown);
	document.getElementById("webViewTranslation").addEventListener("did-navigate", OnWebViewTranslationDidNavigate);
	document.getElementById("webViewTranslation").addEventListener("did-navigate-in-page", OnWebViewTranslationDidNavigateInPage);
	document.getElementById("webViewTranslation").addEventListener("did-frame-finish-load", OnWebViewTranslationDidFrameFinishLoad);
	
	// top level domain 목록을 매번 직접 다운 받는 것은 왠지 네트워크 자원 낭비 같기도 하고, 굳이 매번 갱신할 필요는 없을 것 같으므로 tlds-alpha-by-domain.js 로 만들었다.
	topLevelDomainList = tlds_alpha_by_domain.TLDsAlphaByDomain();
}

function OnNumericUpDownLineNumberInput() {
	const lineNumber = Number(document.getElementById("numericUpDownLineNumber").value);

	if (lineNumber > lastNumericUpDownLineNumberValue)
	{
		TranslateCurrentOrNextLine();
	}
	else if (lineNumber < lastNumericUpDownLineNumberValue)
	{
		TranslateCurrentOrPreviousLine();
	}
	else  // numericUpDownLineNumber의 버튼을 눌렀지만 이전 라인 또는 다음 라인 중에 내용이 있는 라인이 없으면 위 로직에 의해 numericUpDownLineNumber의 값이 원래대로 바뀐다.
	{
		return;
	}

	lastNumericUpDownLineNumberValue = Number(document.getElementById("numericUpDownLineNumber").value);
}

function OnButtonPreviousClick() {
	TranslatePreviousLine();
}

function OnButtonNextClick() {
	TranslateNextLine();
}

function OnCheckBoxClipboardInput() {
	const checkBoxClipboardChecked = document.getElementById("checkBoxClipboard").checked;

	// checkBoxClipboard가 체크되면 1초에 한 번씩 textAreaOriginalText의 내용을 클립보드의 내용으로 덮어씌운다.
	if (checkBoxClipboardChecked)
	{
		interval = setInterval(() => {
			LoadClipboardText();
		}, 1000);

		// 타이머는 1초 후부터 동작하므로, 체크가 됐을 때 일단 한 번 클립보드의 내용을 가져온다.
		LoadClipboardText();
	}
	else
	{
		clearInterval(interval);
	}
}

function OnTextAreaOriginalTextInput() {
	const textAreaOriginalText = document.getElementById("textAreaOriginalText");
	const numericUpDownLineNumber = document.getElementById("numericUpDownLineNumber");
	var lines = textAreaOriginalText.value.split('\n');

	// textAreaOriginalText에 아무것도 입력되어있지 않은 경우
	if (lines.length < numericUpDownLineNumber.min)
	{
		numericUpDownLineNumber.max = numericUpDownLineNumber.min;
		lineOffsetList = null;
		return;
	}

	numericUpDownLineNumber.max = lines.length;

	lineOffsetList = [];
	lineOffsetList[0] = 0;
	for (var i = 1; i < lines.length; i++)
	{
		lineOffsetList[i] = lineOffsetList[i - 1] + lines[i - 1].length + 1;
	}

	OnTextAreaOriginalTextScroll();
	TranslateCurrentOrNextLine();
}

function OnTextAreaOriginalTextScroll() {
	const CHARACTER_WIDTH = 11;
	const LINE_HEIGHT = 32;
	const PADDING_TOP = 26;
	const TEXT_AREA_ORIGINAL_TEXT_CANVAS_LINE_NUMBER_DIFFERENCE = 8;	// 왜 이게 없으면 점점 늘어나는지 모르겠다.
	const textAreaOriginalText = document.getElementById("textAreaOriginalText");
	const canvasLineNumber = document.getElementById("canvasLineNumber");
	const context = canvasLineNumber.getContext("2d");
	var currentPosition = Math.floor(textAreaOriginalText.scrollTop / LINE_HEIGHT, 0);
	
	canvasLineNumber.width = (Math.floor(Math.log10(lineOffsetList.length)) + 1) * CHARACTER_WIDTH;
	canvasLineNumber.height = textAreaOriginalText.clientHeight - TEXT_AREA_ORIGINAL_TEXT_CANVAS_LINE_NUMBER_DIFFERENCE;
	context.font = "20px Yu Gothic";
	for (var i = currentPosition; i < currentPosition + Math.min(Math.ceil(textAreaOriginalText.clientHeight / LINE_HEIGHT, 0), lineOffsetList.length); i++)
	{
		context.fillText(i + 1, 0, PADDING_TOP - textAreaOriginalText.scrollTop + i * LINE_HEIGHT);
	}
}

function OnTextAreaOriginalTextMouseUp(event) {
	if (event.button != 2) {
		return;
	}

	const textAreaOriginalText = document.getElementById("textAreaOriginalText");

	webViewTranslation.loadURL("https://ja.dict.naver.com/#/search?query=" + textAreaOriginalText.value.split('\n')[textAreaOriginalText.value.substr(0, textAreaOriginalText.selectionStart).split('\n').length - 1]);
}

function OnButtonBackClick() {
	document.getElementById("webViewTranslation").goBack();
}

function OnButtonForwardClick() {
	document.getElementById("webViewTranslation").goForward();
}

function OnButtonRefreshClick() {
	document.getElementById("webViewTranslation").reload();
}

function OnButtonStopClick() {
	document.getElementById("webViewTranslation").stop();
}

function OnButtonGoogleTranslateClick() {
	const webViewTranslation = document.getElementById("webViewTranslation");

	// 현재 페이지를 구글 번역기로 번역
	webViewTranslation.loadURL("https://translate.google.com/translate?sl=ja&tl=ko&u=" + encodeURIComponent(webViewTranslation.getURL()));
}

function OnButtonPapagoTranslateClick() {
	const webViewTranslation = document.getElementById("webViewTranslation");

	// 현재 페이지를 파파고로 번역
	webViewTranslation.loadURL("https://papago.naver.net/website?locale=ko&source=ja&target=ko&url=" + encodeURIComponent(webViewTranslation.getURL()));
}

async function OnTextBoxAddressKeyDown(event) {
	const IP_AND_PORT_COUNT = 2;
	const IPv4_NUMBER_COUNT = 4;
	const HTTPS_PORT = 443;

	if (event.keyCode != 13) {
		return;
	}

	const textBoxAddressValue = document.getElementById("textBoxAddress").value.trim();

	// 아무것도 입력되지 않은 경우
	if (!textBoxAddressValue) {
		return;
	}

	const webViewTranslation = document.getElementById("webViewTranslation");

	try {
		// 온전한 URI 형태로 입력되었다면 성공한다.
		await webViewTranslation.loadURL(textBoxAddressValue);
	}
	catch {
		try {
			// 맨 첫 글자가 ?인 경우에는 그 뒤의 단어를 검색어로 간주하고 구글 일본어 검색을 한다.
			if (textBoxAddressValue.startsWith("?")) {
				await webViewTranslation.loadURL("https://www.google.com/search?q=" + encodeURIComponent(textBoxAddressValue.substring(1).trimStart()));
				return;
			}

			// 입력된 문자열을 프로토콜이 HTTP인 URI라고 간주하고 도메인 부분만 분리한다.
			var url = new URL("http://" + textBoxAddressValue);
			var hostAndPort = url.host.split(':');
			var lastIndexOfColon = url.host.lastIndexOf(':');
			var domain;
			var port = hostAndPort[hostAndPort.length - 1];
			var portValid16BitInteger = false;

			if (lastIndexOfColon == -1)
			{
				domain = url.host.split('.');
			}
			else
			{
				var value = Math.floor(Number(port));
	
				if (value !== Infinity && String(value) === port && value >= 0 && value < 65536)
				{
					if (value == HTTPS_PORT)
					{
						url = new URL("https://" + textBoxAddressValue);
					}
					domain = url.host.substring(0, lastIndexOfColon).split('.');
					portValid16BitInteger = true;
				}
				else
				{
					domain = url.host.split('.');
					portValid16BitInteger = false;
				}
			}

			// 호스트가 IPv4 또는 IPv4와 포트로 이루어져 있는 경우를 처리한다.
			if (hostAndPort.length <= IP_AND_PORT_COUNT)
			{
				var valid16BitInteger = true;

				if (hostAndPort.length == IP_AND_PORT_COUNT)
				{
					if (portValid16BitInteger)
					{
						valid16BitInteger = true;
					}
					else
					{
						valid16BitInteger = false;
					}
				}
				
				if (valid16BitInteger)
				{
					var ip = hostAndPort[0];
					var ipNumberList = ip.split('.');

					if (ipNumberList.length == IPv4_NUMBER_COUNT)
					{
						var allValid8BitInteger = true;

						for (var i = 0; i < ipNumberList.length; i++)
						{
							var value = Math.floor(Number(ipNumberList[i]));

							if (value !== Infinity && String(value) === ipNumberList[i] && value >= 0 && value < 256)
							{
								continue;
							}

							allValid8BitInteger = false;
							break;
						}

						if (allValid8BitInteger)
						{
							await webViewTranslation.loadURL(url.href);
							return;
						}
					}
				}
			}

			// 도메인의 맨 마지막이 top level domain 중 하나라면, 입력된 문자열을 프로토콜이 HTTP인 URI로 간주한다. top level domain은 언제라도 추가될 수 있기 때문에 직접 확인한다.
			if (Array.isArray(topLevelDomainList)) {
				// 국제화 도메인일 수도 있으므로 도메인의 맨 마지막을 퓨니코드로 인코딩한다.
				// Javascript 자체에는 퓨니코드를 인코딩하는 라이브러리가 없는 것 같고 https://github.com/bestiejs/punycode.js 이걸 MIT 라이센스로 사용해야 하는 것 같다.
				// var punycodeDomain = punycode.encode(domain[domain.length - 1]);
				var punycodeDomain = domain[domain.length - 1];

				for (var i = 0; i < topLevelDomainList.length; i++) {
					// top level domain 목록의 맨 첫 줄은 무시한다.
					if (topLevelDomainList[i].trimStart().startsWith("#")) {
						continue;
					}

					if (topLevelDomainList[i].toUpperCase() === punycodeDomain.toUpperCase()) {
						await webViewTranslation.loadURL(url.href);
						return;
					}
				}
			}

			// top level domain 목록을 로드하는데 실패했다면 입력된 문자열을 DNS에 질의해 도메인이 존재하는지에 대한 여부를 확인해야 한다. 그리고 top level domain 목록을 로드하는데 성공한 경우, 일반적인 환경에서는 여기까지 왔다면 입력된 문자열이 URI인 경우가 거의 없겠지만, 만약 자체적으로 네임 서버와 도메인을 만들어서 사용하는 환경이라면 URI일 수도 있으므로 DNS 질의는 무조건 할 수밖에 없다. 하지만 입력된 문자열이 URI가 아니라 검색어인 경우, DNS 질의 결과가 온 후에 검색을 진행하면 검색 결과가 나올 때까지 오래 기다리게 된다. 따라서 DNS 질의는 비동기로 하고 질의 결과가 오기 전에 구글 일본어 검색도 동시에 진행한다.
			TryAsURI(url);
			try {
				await webViewTranslation.loadURL("https://www.google.com/search?q=" + encodeURIComponent(textBoxAddressValue));
			}
			catch {
			}
		}
		catch {
			// 입력된 문자열을 프로토콜이 HTTP인 URI로 만드는데 실패했다거나, 빈 문자열을 퓨니코드로 인코딩하려고 시도하다 예외가 발생했다거나 하는 등의 경우에는, 입력된 문자열을 검색어로 간주하고 구글 일본어 검색을 한다.
			try {
				await webViewTranslation.loadURL("https://www.google.com/search?q=" + encodeURIComponent(textBoxAddressValue));
			}
			catch {
			}
		}
	}
}

function OnWebViewTranslationDidNavigate() {
	const webViewTranslation = document.getElementById("webViewTranslation");

	document.getElementById("textBoxAddress").value = webViewTranslation.getURL();
}

function OnWebViewTranslationDidNavigateInPage() {
	const webViewTranslation = document.getElementById("webViewTranslation");

	document.getElementById("textBoxAddress").value = webViewTranslation.getURL();
}

function OnWebViewTranslationDidFrameFinishLoad() {
	const webViewTranslation = document.getElementById("webViewTranslation");

	// 네이버 일본어 사전 검색인 경우에는 번역 결과가 보이도록 무조건 페이지 맨 아래로 스크롤하고, 엔터만 누르면 다음 문장이 검색될 수 있도록 buttonNext에 포커스를 설정한다.
	if (webViewTranslation.getURL().startsWith("https://ja.dict.naver.com/#/search?query="))
	{
		// 뭔가가 실행되어 스크롤이 다시 올라가는 듯 하니 적당히 0.8초 기다린다.
		setTimeout(() => {
			// 검색 이력 때문에 스크롤이 길어지니 검색 이력 삭제처럼 보이는 버튼은 전부 누른 후에 스크롤을 내린다.
			webViewTranslation.executeJavaScript("var elements = document.getElementsByClassName('btn_all_del'); for (var i = 0; i < elements.length; i++) { elements[i].click(); } window.scrollTo(0, document.body.scrollHeight);");
		}, 800);
		document.getElementById("buttonNext").focus();
	}

	document.getElementById("buttonBack").disabled = !webViewTranslation.canGoBack();
	document.getElementById("buttonForward").disabled = !webViewTranslation.canGoForward();
}

function IsNullOrWhiteSpace(string) {
	return !string || string.length === 0 || /^\s*$/.test(string);
}

// 현재 라인을 네이버 일본어 사전으로 검색하고, 현재 라인을 표시한다. 이 함수는 직접 사용하지 않고 TranslatePreviousLine(), TranslateCurrentOrNextLine(), TranslateCurrentOrPreviousLine(), TranslateNextLine()을 통해 사용한다.
function TranslateLine() {
	const textAreaOriginalText = document.getElementById("textAreaOriginalText");
	const lineNumber = document.getElementById("numericUpDownLineNumber").value;
	var lines = textAreaOriginalText.value.split('\n');

	webViewTranslation.loadURL("https://ja.dict.naver.com/#/search?query=" + encodeURIComponent(lines[lineNumber - 1]));
	textAreaOriginalText.focus();
	textAreaOriginalText.setSelectionRange(lineOffsetList[lineNumber - 1], lineOffsetList[lineNumber - 1] + lines[lineNumber - 1].length);
}

// 이전 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 네이버 일본어 사전으로 검색한다. 만약 이전 라인 중에 내용이 있는 라인이 없으면 false를 반환하고, 그렇지 않으면 true를 반환한다.
function TranslatePreviousLine() {
	const textAreaOriginalText = document.getElementById("textAreaOriginalText");
	const numericUpDownLineNumber = document.getElementById("numericUpDownLineNumber");
	var lineNumber = numericUpDownLineNumber.value;
	var lines = textAreaOriginalText.value.split('\n');

	do {
		if (--lineNumber < numericUpDownLineNumber.min) {
			return false;
		}
	}
	while (IsNullOrWhiteSpace(lines[lineNumber - 1]));

	numericUpDownLineNumber.value = lineNumber;
	lastNumericUpDownLineNumberValue = lineNumber;
	TranslateLine();
	return true;
}

// 현재 라인을 네이버 일본어 사전으로 검색한다. 현재 라인에 내용이 없는 경우 다음 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 검색하고, 다음 라인 중에 내용이 있는 라인이 없으면 이전 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 검색한다.
function TranslateCurrentOrNextLine() {
	const textAreaOriginalText = document.getElementById("textAreaOriginalText");
	const numericUpDownLineNumber = document.getElementById("numericUpDownLineNumber");
	const lineNumber = numericUpDownLineNumber.value;
	var lines = textAreaOriginalText.value.split('\n');

	if (lines.length < numericUpDownLineNumber.min) {
		return;
	}

	if (IsNullOrWhiteSpace(lines[lineNumber - 1])) {
		if (!TranslateNextLine()) {
			TranslatePreviousLine();
		}
	} else {
		TranslateLine();
	}
}

// 현재 라인을 네이버 일본어 사전으로 검색한다. 현재 라인에 내용이 없는 경우 이전 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 검색하고, 이전 라인 중에 내용이 있는 라인이 없으면 다음 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 검색한다.
function TranslateCurrentOrPreviousLine() {
	const textAreaOriginalText = document.getElementById("textAreaOriginalText");
	const numericUpDownLineNumber = document.getElementById("numericUpDownLineNumber");
	const lineNumber = numericUpDownLineNumber.value;
	var lines = textAreaOriginalText.value.split('\n');

	if (lines.length < numericUpDownLineNumber.min) {
		return;
	}

	if (IsNullOrWhiteSpace(lines[lineNumber - 1])) {
		if (!TranslatePreviousLine()) {
			TranslateNextLine();
		}
	} else {
		TranslateLine();
	}
}

// 다음 라인 중 내용이 있는 가장 가까운 라인으로 이동한 다음 네이버 일본어 사전으로 검색한다. 만약 다음 라인 중에 내용이 있는 라인이 없으면 false를 반환하고, 그렇지 않으면 true를 반환한다.
function TranslateNextLine() {
	const textAreaOriginalText = document.getElementById("textAreaOriginalText");
	const numericUpDownLineNumber = document.getElementById("numericUpDownLineNumber");
	var lineNumber = numericUpDownLineNumber.value;
	var lines = textAreaOriginalText.value.split('\n');

	do {
		if (++lineNumber > numericUpDownLineNumber.max) {
			return false;
		}
	}
	while (IsNullOrWhiteSpace(lines[lineNumber - 1]));

	numericUpDownLineNumber.value = lineNumber;
	lastNumericUpDownLineNumberValue = lineNumber;
	TranslateLine();
	return true;
}

// 클립보드의 내용이 텍스트이고 textAreaOriginalText와 내용이 다르면, textAreaOriginalText의 내용을 클립보드의 내용으로 덮어씌운다.
function LoadClipboardText() {
	var currentClipboardText = clipboard.readText().replace("\r\n", "\n");

	if (clipboardText == currentClipboardText) {
		return;
	}

	clipboardText = currentClipboardText;
	document.getElementById("textAreaOriginalText").value = clipboardText;
	OnTextAreaOriginalTextInput();
}

// URI의 도메인 부분을 DNS에 질의해 도메인이 존재하는지에 대한 여부를 비동기로 확인한 후 존재하면 해당 웹 페이지를 로드한다.
async function TryAsURI(url) {
	try {
		const webViewTranslation = document.getElementById("webViewTranslation");

		await browser.dns.resolve(uri.host);
		await webViewTranslation.loadURL(url.href);
	}
	catch {
	}
}
