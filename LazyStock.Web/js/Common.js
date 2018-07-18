var Common = function (options) {
    "user strict";
    /* Private Propertites */
    // Create self constant in class
    var self = this;
    // initialize options parameter for definition
    var o = options || {};

    /* Public Propertites */
    //self.data = o.data || null;
    
    /* <public method> */
    self.RoundX = function (x, y) {
        var temp = 1;
        for (i = 0; i < y; i++)
            temp = temp * 10;
        return Math.floor(parseFloat((parseFloat(x))) * temp) / temp;
    }

    /* <public method> */
    self.GenRandom = function (x) {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < x; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    }

    /* <private method>*/
    //var dosomething = function (options) { }

    /* Constructor */
    var __constructor = function () {
        // Construct code
    }
    // Activate constructor
    __constructor();
}