/* eslint-disable */
import store from "@/store";
import router from "@/router";
import MISAEnum from "./enum";
import { AUTH_REQUEST } from "@/store/actions/auth";
import { CLEAR_ERROR_MSG, SET_ERROR_MSG } from '@/store/actions/notification.js';
import { SHOW_LOADING, HIDE_LOADING } from '@/store/actions/loading.js';
import { CLEAR_TOAST, SET_TOAST } from '@/store/actions/toast.js';
const commonJs = {
    login: function(username, password) {
        commonJs.showLoading();
        store
            .dispatch(AUTH_REQUEST, { username, password })
            .then(() => {
                router.push("/contacts");
                commonJs.hideLoading();
            })
            .catch((res) => {
                var msg = null;
                if (res.status == 403) {
                    msg = "Tài khoản của bạn chưa được cấp quyền truy cập tài nguyên hiện tại. Vui lòng liên hệ Mr Mạnh để được cấp quyền."
                    commonJs.showMessenger({
                        title: "Truy cập bị từ chối",
                        msg: msg,
                        type: MISAEnum.MsgType.Error,
                        confirm: () => {
                            router.push("/");
                        }
                    })
                }
                commonJs.hideLoading();
            });
    },
    change_alias: function(alias) {
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
    formatDate: function(value) {
        try {
            if (!value) return "";
            value = new Date(value);
            var date = value.getDate();
            date = date < 10 ? `0${date}` : date;
            var month = value.getMonth() + 1;
            month = month < 10 ? `0${month}` : month;
            var year = value.getFullYear();
            return `${date}/${month}/${year}`;
        } catch (error) {
            return "";
        }
    },
    formatDateTime: function(value) {
        try {
            if (!value) return "";
            value = new Date(value);
            var date = value.getDate();
            date = date < 10 ? `0${date}` : date;
            var month = value.getMonth() + 1;
            month = month < 10 ? `0${month}` : month;
            var year = value.getFullYear();

            var hour = commonJs.addZeroPrefix(value.getHours());
            var min = commonJs.addZeroPrefix(value.getMinutes());
            var second = commonJs.addZeroPrefix(value.getSeconds());
            return `${date}/${month}/${year} \n
                        ${hour}:${min}:${second}`;
        } catch (error) {
            return "";
        }
    },
    addZeroPrefix(number) {
        try {
            if (number == null || number == undefined) return "";
            return number = number < 10 ? `0${number}` : number;
        } catch (error) {
            return "";
        }
    },
    getTime: function(value) {
        try {
            if (!value) return "";
            value = new Date(value);
            var date = value.getDate();
            date = date < 10 ? `0${date}` : date;
            var month = value.getMonth() + 1;
            month = month < 10 ? `0${month}` : month;
            var year = value.getFullYear();

            var hour = value.getHours();
            var datePart = "sáng";
            if (hour >= 18) {
                datePart = "tối";
            } else if (hour <= 18 && hour > 12) {
                datePart = "chiều";
            }
            hour = commonJs.addZeroPrefix(hour);
            var min = commonJs.addZeroPrefix(value.getMinutes());
            var second = commonJs.addZeroPrefix(value.getSeconds());

            return `${hour}:${min} (${datePart})`;
        } catch (error) {
            return "";
        }
    },
    /**
     * Thực hiện định dạng hiển thị tiền tệ dạng VNĐ
     * @param {} record 
     * @param {*} row 
     * @param {*} value 
     * @returns 
     */
    formatMoney: function(value) {
        if (!value) return "";
        try {
            value = new Intl.NumberFormat("vi-VN", {
                style: "currency",
                currency: "VND",
            }).format(value);
            return value;
        } catch (error) {
            return "";
        }
    },
    /**
     * Hiển thị Toast Messenger
     * @param {String} msg 
     * @param {*} type Loại Toast hiển thị
     * Author: NVMANH (02/09/2022) 
     */
    showToast(msg, type) {
        if (!type)
            type = MISAEnum.MsgType.Info;

        store.dispatch(SET_TOAST, { msg: msg, type: type });
        setTimeout(function() {
            store.dispatch(CLEAR_TOAST, {});
        }, 5000)

    },

    /**
     * Hiển thị cảnh báo
     * Authror: NVMANH (16/08/2022)
     */
    showErrorMessenger() {
        var payload = arguments;
        var title = null;
        var msg = [];
        if (payload.length == 0) {
            title = "Thông báo";
            msg.push("Có lỗi xảy ra, vui lòng liên hệ MISA để được trợ giúp.");
        } else if (payload.length == 1) {
            msg.push(payload[0]);
        } else {
            title = payload[0].toString();
            for (let index = 1; index < payload.length; index++) {
                const item = payload[index];
                if (typeof item === 'string' || item instanceof String || typeof item == 'number') {
                    msg.push(item);
                } else if (Array.isArray(item)) {
                    for (const itemChild of item) {
                        if (typeof itemChild === 'string' || itemChild instanceof String || typeof itemChild == 'number') {
                            msg.push(itemChild);
                        }
                    }
                }
            }
        }
        store.dispatch(SET_ERROR_MSG, { title: title, msg: msg, type: MISAEnum.MsgType.Error });
    },


    /**
     * Ẩn cảnh báo
     * Authror: NVMANH (16/08/2022)
     */
    hideErrorMessenger() {
        store.dispatch(CLEAR_ERROR_MSG);
    },

    showConfirm(msg, callback) {
        commonJs.showMessenger({ title: "Xác nhận", msg: msg || "Bạn có chắc chắn muốn thực hiện hành động này?", type: MISAEnum.MsgType.Question, confirm: callback, showCancelButton: true });
    },
    hideConfirm() {
        commonJs.hideMessenger();
    },

    showMessenger({ title, msg, type, confirm, showCancelButton }) {
        store.dispatch(SET_ERROR_MSG, { title: title || "Thông báo", msg: msg || "Có lỗi xảy ra", type: type || MISAEnum.MsgType.Info, confirm: confirm, showCancelButton: showCancelButton | false });
    },

    hideMessenger() {
        store.dispatch(CLEAR_ERROR_MSG);
    },


    showLoading() {
        store.dispatch(SHOW_LOADING);
    },
    showLoadingEl(el) {
        el.classlist.add("loading");
    },
    hideLoading() {
        store.dispatch(HIDE_LOADING);
    }
}
export default commonJs;