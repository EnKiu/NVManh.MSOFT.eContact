class Base {
    constructor() {
        var self = this;
        var args = arguments;
        self.init(args);
    }

    init(args) {
        var self = this;
        for (var i = 0; i < args.length; i++) {
            if ($.type(args[i]) === 'object') {
                var keys = Object.keys(args[0]);
                keys.forEach(function (key) {
                    self[key] = args[i][key];
                });
            }
        }
        self.initEvents();
        self.initCustom();
        self.loadData();
    }

    initEvents() {

    }

    initCustom() {

    }
    loadData() {
        alert('load data for base!');
    }
}
