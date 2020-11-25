$(document).ready(function () {
    contactJS.loadDataToTable();
    // Gán các Even cho các control
    $('#btnSearch').click(contactJS.search);
    $('#txtSearch').keydown(contactJS.txtSearch_onKeyDown);
    $('#btnReload').click(contactJS.reLoadData);
    $('#btnSave').click(contactDetailJS.save);
    $('#btnClose').click(contactJS.hideTitleInfo);
    $('#btnCloseDetail').click(contactJS.hideTitleInfo);
    $('#btnCancel').on('click', contactDetailJS.reset);
    $('#btnUpdateInfo').click(contactDetailJS.changeModeUpdate);
    $('#avatar-box').off('click', function () {
        $('#fuAvatarImage').trigger('click');
    });
    $('#fuAvatarImage').change(contactDetailJS.changeAvatar);
    //$('.modal-content').dblclick(function () {
    //    $('#btnClose').trigger('click');
    //});

    //$('img.avatar').each(function (i, item) {
    //    contactDetailJS.resizeImage(i, item);
    //});
    //$("img.avatar")
    //    .on('load', function () { console.log("image loaded correctly"); })
    //    .on('error', function () { console.log("error loading image"); })
    //    .attr("src", $('img.avatar').attr("src"))
    //;
});

//. đối tượng js phục vụ cho danh bạ
var contactJS = {
    uri: '/api/v1/contacts',
    /* -------------------------------
    * Hàm thực hiện format dữ liệu hiển thị chi tiết cho bảng danh sách khách hàng
    * Created by: NVMANH (03/03/2018)
    * param: sender - đối tượng validate truyền vào
    */
    formatRowItem: function (item) {
        var phoneNumber = item.PhoneNumber ? '</br> ' + item.PhoneNumber : '',
            lastName = item.LastName === 'Mạnh' ? '<span style="color:red;">Mạnh</span>' : item.LastName,
            contactName = item.FirstName + ' ' + '<strong>' + lastName + '</strong>',
            mobile = item.LastName === 'Mạnh' ? '<span style="color:red;">' + item.MobileNumber + (phoneNumber||'') + '</span>' : item.MobileNumber + (phoneNumber||''),
            avatarLink = item.AvatarLink ? item.AvatarLink : '/Content/imgs/avatar.png';

        var tdViewDetail = '<center><button contactid="' + item.ContactId + '" type="button" class="btn btn-info" data-toggle="modal" data-target="#exampleModalCenter" onclick="contactJS.binDataDetail(this)">' +
            'Xem' +
            '</button></center>';
        var html = '<th class="td-avatar" scope="row">' +
            '<img contactid="' + item.ContactId + '" data-toggle="modal" data-target="#exampleModalCenter" onclick="contactJS.binDataDetail(this)" class="th-avatar" src="' + avatarLink + '" alt="Card image cap" align="middle" />' + '</td>' +//+ item.Sort +
            '<td>' + contactName + '</td>' +
            '<td>' + mobile + '</td>';// +
        //'<td>' + item.Address + '</td>' +
        //'<td>' + tdViewDetail + '</td>';
        return html;
    },

    /* -------------------------------
     * Load dữ liệu lấy từ database lên bảng
     * Created by: NVMANH (03/03/2018)
     */
    reLoadData: function () {
        $('#txtSearch').val('');
        contactJS.loadDataToTable();
    },

    /* -------------------------------
     * Load dữ liệu lấy từ database lên bảng
     * Created by: NVMANH (03/03/2018)
     */
    loadDataToTable: function () {
        //setTimeout(function () {
        contactJS.getData($('#tbContactDetail'), true);
        //}, 3000)
    },
    /* -------------------------------
     * Lấy dữ liệu từ Server
     * Created by: NVMANH (03/03/2018)
     * @param <<targetIndicate>>: vùng sẽ hiển thị Indicate
     * @param <<reBuildTable>>: Có buid lại table hay không. (true: có, false: không.)
     */
    getData: function (targetIndicate, reBuildTable) {
        commonJS.showMask(targetIndicate);
        serviceAjax.get(contactJS.uri, {},false, function (data) {
            if (reBuildTable) {
                contactJS.builDataInTable(data);
            }
            localStorage.setItem('storeContact', JSON.stringify(data));
            commonJS.hideMask(targetIndicate);
        });
    },

    builDataInTable: function (data) {
        $('#tbContacts tbody').html('');
        // On success, 'data' contains a list of products.
        $.each(data, function (key, item) {
            // Add a list item for the product.
            $('<tr>', {
                html: contactJS.formatRowItem(item),
                id: item.ContactId,
                ondblClick: 'contactJS.tr_onDblClick(this)'
            }).appendTo($('#tbContacts tbody'));
        });
        $('#tbContacts tbody tr').click(contactJS.setRowSelected);
        commonJS.setFirstRowSelected($('#tbContacts'));
        commonJS.hideMask($('#tbContactDetail'));
    },
    tr_onDblClick: function (sender) {
        var contactId = sender.id;
        $('button[contactId="{0}"]'.format(contactId)).trigger('click');
    },
    /* -------------------------------
     * Set row selected
     * Created by: NVMANH (03/03/2018)
     */
    setRowSelected: function (sender) {
        //sender.stopPropagation();
        //debugger;
        var me = this;
        $('tr').removeClass('rowSelected');
        me.classList.add('rowSelected');
    },
    search: function () {
        var storeContactJSON = localStorage.getItem('storeContact'),
            dataResult;
        if (storeContactJSON) {
            var key = commonJS.change_alias($('#txtSearch').val().replace(' ', '').toLowerCase());

            // nếu từ khóa có ký tự đặc biệt thì tìm kết quả tuyệt đối:
            //if (/^[a-zA-Z0-9- ]*$/.test(key) == false) {
            //    alert('Your search string contains illegal characters.');
            //}

            var storeContact = JSON.parse(storeContactJSON);
            if (key !== '') {
                dataResult = storeContact.filter(function (item) {
                    var fullName = commonJS.change_alias((item.FirstName + item.LastName).replace(' ', '').toLowerCase()),
                        mobile = item.MobileNumber,
                        phone = item.PhoneNumber;
                    return fullName.match(key) || mobile.match(key) || phone.match(key);
                });
            } else {
                dataResult = storeContact;
            }
            contactJS.builDataInTable(dataResult);
        }
    },
    /* -------------------------------
     * bin data detail
     * Created by: NVMANH (03/03/2018)
     */
    binDataDetail: function (sender) {
        $('.title-1').hide();
        contactDetailJS.setModeView();
        var storeContactJSON = localStorage.getItem('storeContact'),
            contactId = sender.getAttribute('contactid');
        contactDetailJS.contactId = contactId;
        if (storeContactJSON) {
            var storeContact = JSON.parse(storeContactJSON),
                contactSelected = storeContact.filter(function (e) {
                    return e.ContactId === contactId;
                })[0];
            contactDetailJS.currentContact = contactSelected;
            // Thực hiện bind các thông tin tương ứng:
            var contactName = contactSelected['FirstName'] + ' ' + contactSelected['LastName'],
                mobile = contactSelected['MobileNumber'],
                phone = contactSelected['PhoneNumber'],
                address = contactSelected['Address'],
                facebook = contactSelected['Facebook'],
                zalo = contactSelected['Zalo'],
                carrer = contactSelected['Career'],
                workPlace = contactSelected['Workplace'],
                otherInfo = contactSelected['OtherInfo'],
                rankStar = contactSelected['RankStar'],
                avatarLink = contactSelected['AvatarLink'];
            avatarLink = avatarLink ? avatarLink : '/Content/imgs/avatar.png';
            $('#avatar').attr('src', avatarLink);

            contactJS.setRankStar(rankStar);
            $('.card-title').text(contactName);
            $('.info-mobile input').val(mobile);
            $('.info-mobile a').attr('href', 'tel:' + mobile);
            if (!phone || phone === '') {
                $('.info-mobile2').hide();
                $('.info-mobile2 input').val(null);
            } else {
                $('.info-mobile2').show();
                $('.info-mobile2 input').val(phone);
                $('.info-mobile2 a').attr('href', 'tel:' + phone);
            }
            $('.li-facebook input').val(facebook);
            $('.li-zalo input').val(zalo);
            $('.li-workPlace span').text(workPlace);
            $('#txtWorkPlace').text(workPlace);
            $('.li-other p').text(otherInfo);
            $('#txtOtherInfo').text(otherInfo);
        }
    },
    setRankStar: function (rankStar) {
        if (!rankStar) {
            rankStar = 1;
        }
        var currentRankClassName = 'rank-star-' + rankStar.toString();
        var rankStarClass = $('.rank-star');
        rankStarClass.removeClass();
        rankStarClass.addClass('rank-star');
        rankStarClass.addClass(currentRankClassName);
    },
    /* -------------------------------
     * Nhấn Enter thực hiện tìm kiếm khi đang ở Textbox Tìm kiếm
     * Created by: NVMANH (03/03/2018)
     */
    txtSearch_onKeyDown: function (event) {
        if (event.which === 13) {
            contactJS.search();
        }
    },
    hideTitleInfo: function () {
        $('.title-1').show();
    }
};

/* ---------------------------------------------------------------
 * Description: đối tượng JS xử lý cho các tác vụ tại foem Detail
 * Author: NVMANH (16/03/2018) 
 */
//var contactDetailJS = new ContactDetailJS();
var contactDetailJS = {
    self: this,
    contactId: null,
    currentContact: null,
    uriFileUpload: "/api/v1/contacts/38e94157-cce9-46c6-bdae-b9928d8ff4e4",
    isUpdate: false,
    /* ---------------------------------------------------------------
     * Description: Mode Edit
     * Author: NVMANH (16/03/2018) 
     */
    changeModeUpdate: function () {
        $('#avatar-box').addClass('avatar-onEditMode');
        $('.modal-content input').removeClass('readonly');
        $('.modal-content input').removeAttr('readonly');
        $('.info-mobile2').show();
        $('.li-workPlace span').hide();
        $('#txtWorkPlace').show();
        $('#txtWorkPlace').removeAttr('hidden');
        $('.li-other p').hide();
        $('#txtOtherInfo').show();
        $('#txtOtherInfo').removeAttr('hidden');
        $('#txtMobile').focus();
        $('#txtMobile').select();
        //Ẩn hiện các Button:
        $('#btnUpdateInfo').hide();
        $('#btnSave').show();
        $('#btnCancel').show();
        $('#btnCancel').removeAttr('hidden');
        $('.btnSaveDetail').bind('click', contactDetailJS.save);
        $('#avatar-box').bind('click', function () {
            $('#fuAvatarImage').trigger('click');
        });

    },

    /* ---------------------------------------------------------------
     * Description: Mode View (Default)
     * Author: NVMANH (16/03/2018) 
     */
    setModeView: function () {
        $('#avatar-box').removeClass('avatar-onEditMode');
        $('.modal-content input').addClass('readonly');
        $('.modal-content input').attr('readonly', '');
        $('.li-workPlace span').show();
        $('#txtWorkPlace').hide();
        $('#txtWorkPlace').attr('hidden', 'hidden');
        $('.li-other p').show();
        $('#txtOtherInfo').hide();
        $('#txtOtherInfo').attr('hidden', 'hidden');
        //Ẩn hiện các Button:
        $('#btnUpdateInfo').show();
        $('#btnSave').hide();
        $('#btnCancel').hide();
        $('#btnCancel').attr('hidden', 'hidden');
        $('.btnSaveDetail').unbind('click');
        $('#avatar-box').unbind('click');
        var phone = $('#txtMobile2').val();
        if (!phone || phone === '') {
            $('.info-mobile2').hide();
        } else {
            $('.info-mobile2').show();
            $('.info-mobile2 input').val(phone);
        }
    },
    /* ---------------------------------------------------------------
     * Description: Reset
     * Author: NVMANH (16/03/2018) 
     */
    reset: function () {
        contactDetailJS.resetDataDetail();
        contactDetailJS.setModeView();
    },

    /* ---------------------------------------------------------------
     * Description: Save info Detail
     * Author: NVMANH (16/03/2018) 
     */
    save: function () {
        if (contactDetailJS.checkChange()) {
            commonJS.showMask($('.modal-content'));
            var formData = new FormData();
            // Thông tin liên hệ:
            var contact = contactDetailJS.buildObjectBeforeSave(),
                file = null;
            var fuAvatarImage = $('#fuAvatarImage');
            if (fuAvatarImage.val() !== '') {
                var files = fuAvatarImage[0];
                file = files.files[0];
            }
            formData.append('file', file);
            formData.append('contactId', contactDetailJS.contactId);
            formData.append('contact', JSON.stringify(contact));
            serviceAjax.putForm(contactDetailJS.uriFileUpload, formData, true, contactDetailJS.afterPostContact);
        } else {
            contactDetailJS.setModeView();
        }
    },

    /* ---------------------------------------------------------------
     * Description: Cập nhật lại ảnh sau khi thay Avatar
     * Author: NVMANH (16/03/2018) 
     */
    changeAvatar: function (sender) {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#avatar').attr('src', e.target.result);
            };
            reader.readAsDataURL(this.files[0]);
            //contactDetailJS.resizeImage2(1, $('#avatar'));
        }
    },
    /* ---------------------------------------------------------------
     * Description: Hàm kiểm tra xem người dùng có Update thông tin gì không, nếu có thì trả về True
     * Author: NVMANH (16/03/2018) 
     */
    checkChange: function () {
        var newContact = contactDetailJS.buildObjectBeforeSave();
        var contactOrginal = JSON.parse(localStorage['storeContact']).find(function (item) { return item.ContactId === contactDetailJS.contactId; });
        contactDetailJS.isUpdate = Object.compare(newContact, contactOrginal);
        return contactDetailJS.isUpdate;
    },

    /* ---------------------------------------------------------------
     * Description: Dựng Object dựa vào các thông tin trước khi thực hiện cất dữ liệu
     * Author: NVMANH (16/03/2018) 
     */
    buildObjectBeforeSave: function () {
        var contact = contactDetailJS.currentContact;
        contact.MobileNumber = $('#txtMobile').val();
        contact.PhoneNumber = $('#txtMobile2').val();
        contact.Facebook = $('#txtFacebook').val();
        contact.Zalo = $('#txtZalo').val();
        contact.Workplace = $('#txtWorkPlace').val();
        contact.OtherInfo = $('#txtOtherInfo').val();
        contact.AvatarLink = $('#avatar').attr('src');
        return contact;

    },

    /* ---------------------------------------------------------------
     * Description: Làm một số việc sau khi gọi Service cất thành công
     * Author: NVMANH (16/03/2018) 
     */
    afterPostContact: function (response) {
        if (response) {
            contactJS.getData($('.modal-content'), false);
            contactDetailJS.commitNewDataAfterSave();
        } else {
            alert(response.Data.responseJSON.ExceptionMessage);
            console.log(response);
        }
        commonJS.hideMask($('.modal-content'));
    },

    /* ---------------------------------------------------------------
     * Description: sau khi cập nhật thành công thực hiện cập nhật các giá trị mới cho Form
     * Author: NVMANH (16/03/2018) 
     */
    commitNewDataAfterSave: function () {
        $('.info-mobile input').val($('#txtMobile').val());

        $('.li-facebook input').val($('#txtFacebook').val());
        $('.li-zalo input').val($('#txtZalo').val());
        $('.li-workPlace span').text($('#txtWorkPlace').val());
        $('.li-other p').text($('#txtOtherInfo').val());
        $('#fuAvatarImage').val('');
        contactDetailJS.setModeView();
        contactDetailJS.showSuccessMessage();
        //Cập nhật lại ảnh đại diện mới ở màn hình list:
        var contactInList = $('.rowSelected').find('img');
        var currentAvatarLink = contactInList.attr('src');
        contactInList.attr('src', currentAvatarLink + '?' + Date.now().toString());
        $('#btnClose').focus();
    },
    /* ---------------------------------------------------------------
     * Description: phục hồi lại dữ liệu đã chỉnh sửa
     * Author: NVMANH (16/03/2018) 
     */
    resetDataDetail: function () {
        var currentContact = contactDetailJS.currentContact;
        $('#txtMobile').val(currentContact.MobileNumber);
        $('#txtFacebook').val(currentContact.Facebook);
        $('#txtZalo').val(currentContact.Zalo);
        $('#txtWorkPlace').val(currentContact.Workplace);
        $('#txtOtherInfo').val(currentContact.OtherInfo);
        if (!currentContact.PhoneNumber || currentContact.PhoneNumber === '') {
            $('.info-mobile2').hide();
        } else {
            $('.info-mobile2').show();
            $('.info-mobile2 input').val(currentContact.PhoneNumber);
        }
        var avatarLink = currentContact['AvatarLink'];
        avatarLink = avatarLink ? avatarLink : '/Content/imgs/avatar.png';
        $('#fuAvatarImage').val('');
        $('#avatar').attr('src', avatarLink);
    },
    /* ---------------------------------------------------------------
     * Description: Hiển thị thông báo Cất thành công.
     * Author: NVMANH (16/03/2018) 
     */
    showSuccessMessage: function () {
        $('.box-success-message').removeAttr('hidden');
        $('.box-success-message').show();
        $('.success-message-content').slideDown(1000);
        setTimeout(function () {
            $('.success-message-content').slideUp(1000);
            $('.box-success-message').attr('hidden', 'hidden');
            $('.box-success-message').hide();
        }, 1200);
    }
};