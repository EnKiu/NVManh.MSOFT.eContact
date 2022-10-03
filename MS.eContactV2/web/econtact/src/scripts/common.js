/* eslint-disable */
const commonJs = {
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
}
export default commonJs;