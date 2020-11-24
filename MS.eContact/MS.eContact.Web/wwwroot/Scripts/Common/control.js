
class Dialog {
    constructor(element, width, height, scope) {
        var me = this;
        this.Dialog = $(element).dialog({
            autoOpen: false,
            width: width,
            modal: true,
            buttons: this.InitButtons(),
            close: function () {
                //me.form[0].reset();
            }
        });
    }
    InitButtons() {
        var me = this;
        var buttons = [
            {
                text: "Đăng ký",
                class: "btn btn-primary btnRegisterEvent",
                click: me.showRegisterForm
            },
            {
                text: "Hủy bỏ",
                class: "btn btn-primary",
                click: function () {
                    me.dialogDetail.dialog("close");
                }
            }
        ]
        return buttons;
    }
    Show() {

    }
    Close() {

    }
}