﻿
$(document).ready(function () {

});

var commonJS = {
    showMask: function (sender) {
        sender.addClass('loading');
    },
    hideMask: function (sender) {
        sender.removeClass('loading');
    },
    /* -----------------------------------------
     * Hiển thị hộp thoại cảnh báo
     * Created by: NVMANH (03/03/2018)
     */
    showWarning: function (msg) {

    },
    /* -----------------------------------------
     * Hiển thị hộp thoại xác nhận xóa dữ liệu
     * Created by: NVMANH (03/03/2018)
     */
    showConfirm: function (msg, confirmCallBack) {
        var html = '<div class="question-content"><div class="question-icon"></div>Bạn có thực sự muốn xóa khách hàng đã chọn?</div>' +
            '<div class="dialog-message-bottom-toolbar">' +
            '<button id="btnConfirm">Đồng ý</button>' +
            '<button id="btnCancel">Hủy bỏ</button>' +
            '</div>';
        $('#dialog-message').html(html);
        $(function () {
            $("#dialog-message").dialog({
                modal: true,
                resizable: false,
                class: "bottom-dialogmessage",
                width: 350,
            });
            confirmCallBack = (function () {
                var cached_function = confirmCallBack;
                return function () {
                    $('#dialog-message').dialog("close")
                    var result = cached_function.apply(this, arguments); // use .apply() to call it
                    return result;
                };
            })();
            //Gán Even cho các Button:
            $('.dialog-message-bottom-toolbar #btnConfirm').click(confirmCallBack);
            $('.dialog-message-bottom-toolbar #btnCancel').click(function () { $('#dialog-message').dialog("close") });
        });
    },

    /* -----------------------------------------
     * Hiển thị hộp thoại thông báo thành công
     * Created by: NVMANH (03/03/2018)
     */
    showSuccessMsg: function () {
        $('body').append('<div class="msg-success" style="display:none">Thành công</div>');
        $('.msg-success').slideDown(1000);
        setTimeout(function () {
            $('.msg-success').slideUp(1000);
        }, 3000);
    },
    change_alias: function (alias) {
        var str = alias;
        str = str.toLowerCase();
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
        str = str.replace(/ + /g, " ");
        str = str.trim();
        return str;
    },

    /* --------------------------------------------
     * Select vào dòng đầu tiên trong bảng dữ liệu
     * Created by: NVMANH (03/03/2018)
     */
    setFirstRowSelected: function (table) {
        var tBodys = table[0].tBodies,
            firstRow = null;
        if (tBodys.length > 0) {
            var tBody = tBodys[0],
                rows = tBody.rows;
            firstRow = rows.length > 0 ? rows[0] : null;
        }
        if (firstRow) {
            firstRow.classList.add('rowSelected');
        }
    },

    /* -------------------------------------------------------------------------------
     * Hiển thị cảnh báo khi validate dữ liệu trống (các trường yêu cầu bắt buộc nhập)
     * Created by: NVMANH (03/03/2018)
     */
    validateEmpty: function (sender) {
        var target = sender.target,
            idEmpty = target.id + '-empty';
        value = target.value,
            parent = $(this).parent(),
            currentThisWith = $(this).width();

        if (!value || value === '') {
            target.classList.add('validate-error');
            if (parent.find('.divError').length === 0) {
                parent.append('<div id="' + idEmpty + '" class="divError" title="Không được để trống trường này"></div>');
            }
        } else {
            target.classList.remove('validate-error');
            target.title = "";
            $('#' + idEmpty).remove();
        }
    }
};


if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] !== 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}

if (!Object.compare) {
    Object.compare = function (obj1, obj2) {
        var isDifferent = false;
        for (var property in obj1) {
            if (obj1[property] !== obj2[property]) {
                isDifferent = true;
                break;
            }
        }
        return isDifferent;
    };
}

if (!Date.prototype.ddmmyyyy) {
    Date.prototype.ddmmyyyy = function () {
        var mm = this.getMonth() + 1; // getMonth() is zero-based
        var dd = this.getDate();

        return [(dd > 9 ? '' : '0') + dd + '/',
        (mm > 9 ? '' : '0') + mm + '/',
        this.getFullYear()
        ].join('');
    };
}

if (!Date.prototype.formatddMMyyyy) {
    Date.prototype.formatddMMyyyy = function () {
        var day = this.getDate();
        var month = this.getMonth() + 1;
        var year = this.getFullYear();
        return day + '/' + month + '/' + year;
    };
}

if (!Date.prototype.formatMMddyyy) {
    Date.prototype.formatMMddyyyy = function () {
        var day = this.getDate();
        var month = this.getMonth() + 1;
        var year = this.getFullYear();
        return month + '-' + day + '-' + year;
    };
}

if (!Date.prototype.formatyyyyMMdd) {
    Date.prototype.formatyyyyMMdd = function () {
        var day = this.getDate();
        var month = this.getMonth() + 1;
        var year = this.getFullYear();
        return year + '-' + month + '-' + day;
    };

}


if (!Number.prototype.formatMoney) {
    Number.prototype.formatMoney = function () {
        return this.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
    };
}

