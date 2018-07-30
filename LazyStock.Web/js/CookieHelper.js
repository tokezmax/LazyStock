/**
 * Cookie Helper
 * Copyright 2018 LeonChen
 */
var CookieHelper = function (options) {
    "user strict";
    /* Private Propertites */
    // Create self constant in class
    var self = this;
    // initialize options parameter for definition
    var o = options || {};
	
    /* Public Propertites */
    
    /* <public method> */
    self.setCookie =function (name,value,expiryMin)
	{
		if(CookieIsDisable())
			window.localStorage.setItem(name, value);

		var exp  = new Date(); 
		exp.setTime(exp.getTime() + expiryMin*1000);
		document.cookie = name + "="+ escape (value) + ";expires=" + exp.toGMTString();
		return true;
	}
	
	self.getCookie =function (name)
	{
		if(CookieIsDisable() &&window.localStorage[name])
			return window.localStorage[name];
		
		var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));
		if(arr != null) return unescape(arr[2]); return null;
	}
	
	self.delCookie =function (name)
	{
		if(CookieIsDisable() &&window.localStorage[name]){
			window.localStorage.removeItem(name);
			return true;
		}
			
		var exp = new Date();
		exp.setTime(exp.getTime() - 1);
		var cval=self.getCookie(name);
		if(cval!=null) document.cookie= name + "="+cval+";expires="+exp.toGMTString();
		
		return true;
	}


    /* <private method>*/
	var CookieIsDisable = function () {
		return (!(document.cookie || navigator.cookieEnabled));
    }
	
	
    /* Constructor */
    var __constructor = function () {
        // Construct code
    }
    // Activate constructor
    __constructor();
}