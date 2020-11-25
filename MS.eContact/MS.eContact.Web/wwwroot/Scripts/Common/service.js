var serviceAjax = {
    /**
     * Thực hiện thêm mới các thuộc tính vào Request header
     * Author: NVMANH (11/05/2019)
     * */
    setRequestHeader: function () {
        var tokenKey = 'accessToken';
        var token = sessionStorage.getItem(tokenKey);
        var headers = { 'X-Requested-With': 'XMLHttpRequest' };
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        return headers;
    },
    getJson: function (uri, callback) {
        $.getJSON(uri)
            .done(function (data) {
                if (callback) {
                    callback(data, true);
                }
            });
    },
    /**
    * Lấy dữ liệu
    * @param {string} uri
    * @param {JSON} param
    * @param {bool} async
    * @param {function} callback
    * Author: NVMANH (11/05/2019)
    */
    get: function (uri, param, async, callback) {
        var headers = serviceAjax.setRequestHeader();
        $.ajax({
            url: uri,
            dataType: 'json',
            async: async ? async : false,
            data: param,
            headers: headers
        }).done(function (data) {
            if (callback) {
                callback(data);
            }
        }).fail(function (res) {
            serviceAjax.doAfterFailAjaxRequest(res);
        });

    },

    /**
    * Thêm mới dữ liệu 
    * @param {string} uri
    * @param {JSON} param
    * @param {bool} async
    * @param {function} callback
    * Author: NVMANH (11/05/2019)
    */
    post: function (uri, param, async, callback) {
        var headers = serviceAjax.setRequestHeader();
        $.ajax({
            url: uri,
            type: 'POST',
            data: param,
            contentType: 'application/json',
            async: async === undefined ? false : async,
            headers: headers
        }).done(function (res) {
            if (callback) {
                callback(res);
            }
        }).fail(function (res) {
            serviceAjax.doAfterFailAjaxRequest(res);
        });
    },

    /**
    * Thêm mới dữ liệu kèm theo các File
    * @param {string} uri
    * @param {JSON} param
    * @param {bool} async
    * @param {function} callback
    * Author: NVMANH (11/05/2019)
    */
    postForm: function (uri, param, async, callback) {
        var headers = serviceAjax.setRequestHeader();
        $.ajax({
            url: uri,
            type: 'POST',
            data: param,
            contentType: false,
            processData: false,
            async: async === undefined ? false : async,
            headers: headers
        }).done(function (res) {
            if (callback) {
                callback(res);
            }
        }).fail(function (res) {
            serviceAjax.doAfterFailAjaxRequest(res);
        });
    },

    /**
    * Thêm mới dữ liệu kèm theo các File
    * @param {string} uri
    * @param {JSON} param
    * @param {bool} async
    * @param {function} callback
    * Author: NVMANH (11/05/2019)
    */
    putForm: function (uri, param, async, callback) {
        var headers = serviceAjax.setRequestHeader();
        $.ajax({
            url: uri,
            type: 'PUT',
            data: param,
            contentType: false,
            processData: false,
            async: async === undefined ? false : async,
            headers: headers
        }).done(function (res) {
            if (callback) {
                callback(res);
            }
        }).fail(function (res) {
            serviceAjax.doAfterFailAjaxRequest(res);
        });
    },

    /**
     * Thực hiện sửa dữ liệu
     * @param {string} uri
     * @param {JSON} param
     * @param {bool} async
     * @param {function} callback
     * Author: NVMANH (11/05/2019)
     */
    put: function (uri, param, async, callback) {
        var headers = serviceAjax.setRequestHeader();
        $.ajax({
            url: uri,
            type: 'PUT',
            data: param,
            //dataType: 'application/json',
            async: async === undefined ? false : async,
            headers: headers
        }).done(function (res) {
            if (callback) {
                callback(res);
            }
        }).fail(function (res) {
            serviceAjax.doAfterFailAjaxRequest(res);
        });
    },

    /**
     * Thực hiện xóa dữ liệu
     * @param {string} uri
     * @param {JSON} param
     * @param {bool} async
     * @param {function} callback
     * Author: NVMANH (11/05/2019)
     */
    delete: function (uri, param, async, callback) {
        var headers = serviceAjax.setRequestHeader();
        $.ajax({
            url: uri,
            type: 'DELETE',
            data: param,
            async: async === undefined ? false : async,
            headers: headers
        }).done(function (res) {
            if (callback) {
                callback(res);
            }
        }).fail(function (res) {
            serviceAjax.doAfterFailAjaxRequest(res);
        });
    },

    /**
     * Hàm thực hiện sau khi Ajax Request lỗi
     * @param {any} res
     */
    doAfterFailAjaxRequest: function (res) {
        if (res.status === 401) {
            var returnPath = encodeURIComponent(window.location.pathname);
            window.location.replace("/Account/Login?returnUrl=" + returnPath);
        }
        $('.loading').removeClass('loading');
    }
};