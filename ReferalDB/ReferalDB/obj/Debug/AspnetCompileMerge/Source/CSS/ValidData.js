function EmailRegx(EmaitText) {
    var regx = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

    if (!EmaitText.match(regx)) {      
        return false;
    }
    else {
        return true;
    }
}

function PhoneRegx(PhoneText) {
    var regx = /^\([0-9]{3}\)[0-9]{3}\-[0-9]{4}$/;
    
    if (!PhoneText.match(regx)) {
        return false;
    }
    else {
        return true;

    }
}
