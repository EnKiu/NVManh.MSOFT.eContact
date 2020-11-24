class Account {
    constructor() {
        this.UserName = $("").val();
        this.Password = $("").val();
        this.TokenKey = "accessToken";
        this.InitEvents();
        this.CheckAuth();
    }
    InitEvents() {
        $('form#frmLogin').on('submit', this.Login.bind(this));
        $('form#frmRegister').on('submit', this.Register.bind(this));
        $('#login-form').on('click', '#linkRegister', this.ShowRegisterForm.bind(this));
        $('#register-form').on('click', '#linkLogin', this.ShowLoginForm.bind(this));
        $('#navUserInfo').on('click', '#navLogout', this.Logout.bind(this));
        this.SetCustomValidity();
    }
    /**
     * Tùy chỉnh lại việc validate dữ liệu của Bootstrap
     * @param {any} sender
     */
    SetCustomValidity(sender) {
        var msg = "";
        var elements = document.getElementsByTagName("input");

        for (var i = 0; i < elements.length; i++) {
            elements[i].oninvalid = function (e) {
                if (!e.target.validity.valid) {
                    switch (e.target.name) {
                        case 'firstname':
                            e.target.setCustomValidity("Họ không được để trống");
                            break;
                        case 'lastname':
                            e.target.setCustomValidity("Tên không được để trống");
                            break;
                        case 'mobile':
                            e.target.setCustomValidity("Số điện thoại không được để trống");
                            break;
                        case 'password':
                            e.target.setCustomValidity("Mật khẩu không được để trống");
                            break;
                        default:
                            e.target.setCustomValidity("");
                            break;

                    }
                }
            };
            elements[i].oninput = function (e) {
                e.target.setCustomValidity(msg);
            };
        }
    }

    /**
     * Kiểm tra lại thông tin người dùng
     * */
    CheckAuth() {
        var partUrl = window.location.pathname;
        if (partUrl !== "/Account/Login") {
            var headers = serviceAjax.setRequestHeader();
            $.ajax({
                url: "/api/Account/UserInfo",
                dataType: 'json',
                async: false,
                headers: headers
            }).done(function (res) {
                $('#lblUserName').text(res["Phone"]);
                $('#navLogin').hide();
                $('#navUserInfo').show();
            }).fail(function (res) {
                $('#navLogin').show();
                $('#navUserInfo').hide();
                serviceAjax.doAfterFailAjaxRequest(res);
            });
        }
    }

    /**
     * Thực hiện đăng nhập hệ thống
     * @param {any} sender
     * @param {any} e
     */
    Login(sender, e) {
        var self = this;
        if (sender) {
            sender.preventDefault();
        }
        var loginData = {
            grant_type: 'password',
            username: $('#login-form input[name="username"]').val(),//self.loginEmail(),
            password: $('#login-form input[name="password"]').val()//self.loginPassword()
        };
        commonJS.showMask($('body'));

        $.ajax({
            type: 'POST',
            url: '/Token',
            async: false,
            contentType: 'application/json; charset=utf-8',
            data: loginData
        }).done(function (res) {
            // Cache the access token in session storage.
            sessionStorage.setItem(self.TokenKey, res.access_token);
            sessionStorage.setItem('username', loginData.username);
            var returnUrl = decodeURIComponent(window.location.search);
            returnUrl = returnUrl.replace("?returnUrl=", "");
            window.location.href = window.location.origin + returnUrl;
            $('#login-form .showErrorValid').hide();
            commonJS.hideMask($('body'));
        }).fail(function (res) {
            $('#login-form .showErrorValid').html('');
            if (res.status === 400) {
                $('#login-form .showErrorValid').append(res.responseJSON['error_description']);
                $('#login-form .showErrorValid').show();
            }
            showError(res);
            commonJS.hideMask($('body'));
        });
    }

    /**
     * Đăng xuất khỏi hệ thống
     * */
    Logout() {
        var self = this;
        commonJS.showMask($('body'));
        serviceAjax.post('/api/Account/Logout', {}, false, function (data) {
            // Successfully logged out. Delete the token.
            sessionStorage.removeItem(self.TokenKey);
            window.location.replace("/Account/Login");
        });
    }

    /**Hiển thị form đăng nhập */
    ShowLoginForm() {
        $('#login-form .showErrorValid').html('');
        $('#register-form .showErrorValid').hide();
        $('#login-form').show();
        $('#register-form').hide();
        $('#login-form input[name="username"]').focus();
    }

    /**Hiển thị form đăng ký */
    ShowRegisterForm() {
        $('#register-form input').val(null);
        $('#login-form .showErrorValid').html('');
        $('#register-form .showErrorValid').hide();
        $('#login-form').hide();
        $('#register-form').show();
        $('#register-form input[name="firstname"]').focus();
    }

    /**
     * Thực hiện Đăng ký tài khoản mới
     * @param {any} sender
     */
    Register(sender) {
        var me = this;
        sender.preventDefault();
        var data = {
            FirstName: $('#register-form input[name="firstname"]').val(),
            LastName: $('#register-form input[name="lastname"]').val(),
            Mobile: $('#register-form input[name="mobile"]').val(),
            Email: $('#register-form input[name="username"]').val(),
            Password: $('#register-form input[name="password"]').val(),
            ConfirmPassword: $('#register-form input[name="confirmpassword"]').val()
        };
        commonJS.showMask($('body'));
        $.ajax({
            type: 'POST',
            url: '/api/Account/Register',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        }).done(function (res) {
            $('.showErrorValid').hide();
            $('#login-form input[name="username"]').val(data.Mobile);
            $('#login-form input[name="password"]').val(data.Password);
            $('#login-form').show();
            $('#register-form').hide();
            me.Login();
            commonJS.hideMask($('body'));
        }).fail(function (res) {
            showError(res);
            commonJS.hideMask($('body'));
        });
    }
}
accountJS = new Account();

function showError(jqXHR) {
    $('.showErrorValid ul').html('');
    var response = jqXHR.responseJSON;
    if (response) {
        if (response.ModelState) {
            var modelState = response.ModelState;
            for (var prop in modelState) {
                if (modelState.hasOwnProperty(prop)) {
                    var msgArr = modelState[prop]; // expect array here
                    if (msgArr.length) {
                        for (var i = 0; i < msgArr.length; ++i) {
                            $('.showErrorValid ul').append('<li>' + msgArr[i] + '</li>');
                        }
                    }
                }
            }
        }
        $('#register-form .showErrorValid').show();
    }
}


