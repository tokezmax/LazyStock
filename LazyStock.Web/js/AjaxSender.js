var Ajax = function (options) {
    "user strict";
    /* Private Propertites */
    // Create self constant in class
    var self = this;

    // initialize options parameter for definition
    var o = options || {};
    // Flag for loadData()
    var isLoading = false;

    /* Public Propertites */
    // Tree data
    self.data = o.data || null;
    // AJAX URL for loadData()
    self.ajaxUrl = o.ajaxUrl || '/org3layermenu/ajax';

    /* Constructor */
    var __constructor = function () {

        // Construct code
    }

    /* <public method> Initialize with callback function */
    self.init = function (callback) {

        loadData(callback);
    }

    /* <public method> Render */
    self.render = function (options) {
        // Load data if no data 
        if (self.data === null) {
            loadData(function () {
                self.render(options)
            });
            return;
        }
        console.log(self.data);
        // Render code
    }

    /* <private method> Load data from AJAX */
    var loadData = function (callback) {

        // Check if is loading
        if (isLoading) {
            // Retry later
            setTimeout(function () {
                callback();
            }, 500);
            return;
        }

        isLoading = true;
        // Load data by AJAX
        $.get(self.ajaxUrl, function (data) {
            self.data = data;
            isLoading = false;
            callback();
        });
    }

    // Activate constructor
    __constructor();
}