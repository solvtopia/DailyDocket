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
function openModuleWizard(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onModuleWizardClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}

function openModule(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onModuleClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}

function openLogin(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onLoginClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}

function openProfile(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onProfileClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}

function openNewsEditor(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onNewsEditorClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}

function openAdminUsers(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onAdminUsersClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}

function openReports(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onReportsClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}

function openConfirmation(pgUrl, windowCtrl) {
    var oWnd = radopen(pgUrl, windowCtrl);
}

function onConfirmationClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {
        var result = arg.result;
        //alert(result);
    }

    top.location.href = top.location.href;
}

