// finds the radwindow on the page
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function forceRadWindow(url) {
    var oWnd = GetRadWindow();
    if (oWnd == null) {
        location.href = url;
    }
}

function returnToParent(value, url) {
    //create the argument that will be returned to the parent page
    var oArg = new Object();

    //get the arguments
    oArg.newGood = value;
    if (url != '') {
        oArg.Url = url;
    }

    //get a reference to the current RadWindow
    var oWnd = GetRadWindow();

    //Close the RadWindow and send the argument to the parent page
    oWnd.close(oArg);
}




// *************************************************************************************************************************************************
// workers for individual popups
// *************************************************************************************************************************************************
function openInformationWindow(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onInformationWindowClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}
