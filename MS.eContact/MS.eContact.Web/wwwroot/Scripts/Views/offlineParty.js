$(document).ready(function () {
    eventPagesJS = new EventsPageJS({
        listEvents: $('#listEvent'),
    });
    //masterForm.init({
    //    listEvents: $('#listEvent'),
    //}),
    detailForm.init({
        cboRegisterSelect: $('#cboRegisterSelect'),
        btnShowDetail: $('.offline-party-item'),
        btnRegister: $('#btnRegister'),
        formDetail: $('#dialog-form'),
        frmRegister: $('#dialog-register'),
        eventDetailContent: $('#event-content'),
        eventTime: $('#event-time'),
        eventPlace: $('#event-place'),
        tblDetails: $('#members tbody'),
        numPerson: $('#numPerson'),
    });
})
class EventsPageJS extends Base {
    /*-----------------------------------
     * Thực hiện gán Event cho các element cần thiết
     * Createdby: NVMANH (23/04/2019)
     * */
    initEvents() {
        $(document).on('click', '#btnRegisterEvent', this.showDetailEvent);
    }
    initCustom() {
        var a = new Dialog("#dialog-form", 200, 200);
    }
    /* ----------------------------------
     * Thực hiện lấy dữ liệu
     * Author: NVMANH (29/04/2018)
     */
    loadData() {
        var me = this;
        commonJS.showMask($('body'));
        serviceAjax.get('/api/Event', {}, false, function (result) {
            if (result.Success) {
                var listEvents = me.listEvents;
                listEvents.html('');
                // On success, 'data' contains a list of products.
                $.each(result.Data, function (key, item) {
                    var eventDate = new Date(item.EventDate);
                    var eventDateDisplay = eventDate.formatddMMyyyy(),
                        spend = item.Spends.formatMoney() + ' đ';
                    var eventStatusText = '<i class="text-active">Đang diễn ra</i>';

                    // event has finished or not yet:
                    var isCommingSoon = true;
                    if (eventDate > new Date()) {
                        eventStatusText = '<i class="text-active">Sắp diễn ra</i>';
                    } else if (eventDate < new Date()) {
                        isCommingSoon = false;
                        eventStatusText = '<i class="text-blur">Đã xong</i>';
                    } else {
                        eventStatusText = '<i class="text-active">Đang diễn ra</i>';
                    }

                    var htmlEvent = '<div class="card-body">'
                        + '<h5 class="card-title title-link">{0}</h5>'
                        + '    <p class="card-text">{1}</p>'
                        + '    <div class="event-info">'
                        + '        <div class="event-info-time">'
                        + '            <div class="event-info-item-icons event-info-time-icons"></div>'
                        + '            <div class="event-info-time-content">'
                        + '                <div class="event-text-content-item">{2} (Bắt đầu lúc {5})</div>'
                        + '                {6}'
                        + '            </div>'
                        + '        </div>'
                        + '        <div class="event-info-place">'
                        + '            <div class="event-info-item-icons event-info-place-icons"></div>'
                        + '            <div class="event-info-place-content">'
                        + '                <div class="event-text-content-item">{3}</div>'
                        + '                <i class="text-blur">{7}</i>'
                        + '            </div>'
                        + '        </div>'
                        + '        <div class="event-info-price">'
                        + '            <div class="event-info-item-icons event-info-price-icons"></div>'
                        + '            <div class="event-info-price-content event-text-content-item">{4}</div>'
                        + '        </div>'
                        + '    </div>'
                        + '    <hr />';

                    // Button is disabled if event finish:
                    var buttonRegisterHTML = $('<button id="btnRegisterEvent" href="#" class="btn btn-primary "'
                        + (!isCommingSoon ? "disabled" : "") + '>Xem chi tiết & Đăng ký</button>').data(item);

                    // Build full html of Event:
                    htmlEvent = htmlEvent.format(
                        item.EventName,
                        item.EventContent,
                        eventDateDisplay,
                        item.EventPlace,
                        spend,
                        item.StartTimeText,
                        eventStatusText,
                        item.EventAddress
                    );

                    // Append to list event on page:
                    $('<div>', {
                        html: $(htmlEvent).append(buttonRegisterHTML),
                        eventId: item.EventId,
                        class: "row card event-card"
                    }).appendTo(listEvents);
                });
            }
            commonJS.hideMask($('body'));
        });
    }
    /* -----------------------------------------
     * Hiển thị thông tin chi tiết sự kiện
     * @param {any} event
     * Createdby: NVMANH (23/04/2019)
     */
    showDetailEvent(event) {
        var eventId = $(this).data()['EventId'];
        detailForm.masterId = eventId;
        detailForm.loadData();
        detailForm.dialogDetail.dialog("open");
    }
}

/**
 * JS Object của form chi tiết sự kiện
 * Author: NVMANH (29/04/2018)
 */
var detailForm = Object.create({
    dialogDetail: null,
    masterId: null,
    form: null,
    init: function () {
        var me = this,
            args = arguments;
        for (var i = 0; i < args.length; i++) {
            if ($.type(args[i]) === 'object') {
                var keys = Object.keys(args[0]);
                keys.forEach(function (key) {
                    me[key] = args[i][key];
                });
            }
            //$('#cboRegisterSelect').on('change', function () {
            //    var currentContactId = event.target.value;
            //    alert(currentContactId);
            //})
        }
        // Khởi tạo form đăng ký:
        me.dialogRegister = $("#dialog-register").dialog({
            autoOpen: false,
            //height: 250,
            width: 300,
            modal: true,
            buttons: {
                "Xác nhận": me.register,
                "Hủy bỏ": function () {
                    me.dialogRegister.dialog("close");
                }
            },
            close: function () {
                me.formRegister[0].reset();
            }
        });

        //Khởi tạo form chi tiết
        me.dialogDetail = $("#dialog-form").dialog({
            autoOpen: false,
            height: 608,
            width: 345,
            modal: true,
            buttons: [
                {
                    text: "Đăng ký",
                    class: "btn btn-primary btnRegisterEvent",
                    click: detailForm.showRegisterForm,
                },
                {
                    text: "Hủy bỏ",
                    class: "btn btn-primary",
                    click: function () {
                        me.dialogDetail.dialog("close");
                    }
                }
            ],
            close: function () {
                me.form[0].reset();
            }
        });

        me.form = me.dialogDetail.find("form").on("submit", function (event) {
            event.preventDefault();
            me.register();
        });



        me.formRegister = me.dialogRegister.find("form").on("submit", function (event) {
            event.preventDefault();
            me.register();
        });
    },
    loadData: function () {
        var eventId = detailForm.masterId;
        var uri = '/api/events/' + eventId;
        serviceAjax.get(uri, {}, false, function (result) {
            if (result.Success) {
                detailForm.masterId = eventId;
                detailForm.bindingData(result.Data);
            }
            commonJS.hideMask($('.ui-dialog'));
        });
        setTimeout(function () {
            detailForm.resizeTable();
        }, 0);
    },
    /* ---------------------------------------------------
     * Resize lại các cột dữ liệu
     * Author: NVManh (29/04/2018)
     */
    resizeTable: function () {
        var tblDetails = detailForm.tblDetails;
        var tbody = tblDetails[0],
            scrollHeight = tbody.scrollHeight,
            clientHeight = tbody.clientHeight;
        if (scrollHeight > clientHeight) {
            detailForm.tblDetails.find('.col-03-tbody').css('width', '20px');
        } else {
            detailForm.tblDetails.find('.col-03-tbody').css('width', '41px');
        }
    },
    /* ---------------------------------------------------
     * Bin dữ liệu cho danh sách chi tiết thành viên đăng ký
     * Author: NVManh (29/04/2018)
     */
    bindingData: function (data) {
        var event = data,
            eventDate = (new Date(event['EventDate'])).ddmmyyyy(),
            eventDetails = event['EventDetails'],
            tblDetails = detailForm.tblDetails;
        detailForm.formDetail.title = '';
        debugger;
        detailForm.eventDetailContent.text(event['EventContent']);
        detailForm.eventTime.text(event['StartTimeText'].replace('AM', 'sáng').replace('PM', 'chiều'));
        detailForm.eventPlace.text(event['EventPlace']);

        var fields = $('[field-data]');
        $.each(fields, function (index, item) {
            var fieldName = $(item).attr('field-data');
            var fieldValue = data[fieldName];
            var dataType = $.type(fieldValue);
            if (dataType === 'string' && Date.parse(fieldValue)) {//dataType instanceof Date
                dataType = "date";
            }
            switch (dataType) {
                case "number":
                    fieldValue = fieldValue.formatMoney() + ' đ';
                    break;
                case "date":
                    // Thực hiện format dạng ngày/tháng/năm:
                    fieldValue = new Date(fieldValue).formatddMMyyyy();
                    break;
                default:
                    break;
            }
            $(item).text(fieldValue);
        });
        $('.event-name').html(event['EventName']);
        if (eventDetails && eventDetails.length > 0) {
            $('#totalMember').text(eventDetails.length);
            tblDetails.html('');

            // On success, 'data' contains a list of products.
            var count = 1;
            $.each(eventDetails, function (key, item) {
                // Add a list item for the product.
                var lastName = item.LastName === 'Mạnh' ? '<span style="color:red;">Mạnh</span>' : item.LastName,
                    fullName = item.FullName;
                $('<tr>', {
                    html: '<td class="col-01-tbody">{3}</td><td class="col-02-tbody"><b>{0}</b> ({1})</td><td class="col-03-tbody">{2}</td>'.format(lastName, fullName, item.NumberAccompanying, count),
                    value: item.ContactId
                }).appendTo(tblDetails);
                count++;
            });
        } else {
            $('#totalMember').text(0);
            tblDetails.html('<tr><td colspan="3"><i>Chưa có thành viên nào đăng ký</i></td></tr>');
        }
    },

    /* ---------------------------------------------------
     * Hiển thị Form đăng ký
     * Author: NVManh (29/04/2018)
     */
    showRegisterForm: function () {
        // Lấy danh sách các thành viên chưa đăng ký:
        serviceAjax.get('/contacts/{0}/{1}'.format(detailForm.masterId, sessionStorage.getItem('username')), {}, true, function (result) {
            if (result.Success) {
                detailForm.buildDataForComboboxRegisterSelect(result.Data['ListContact']);
                var contactInfo = result.Data['ContactInfo'];
                console.log(contactInfo['ContactId']);
                if (contactInfo && contactInfo['ContactId']) {
                    detailForm.cboRegisterSelect.val(contactInfo['ContactId']);
                } else {
                    detailForm.cboRegisterSelect.val(1);
                }
                
                // Binding dữ liệu vào combobox chọn:
                detailForm.dialogRegister.dialog("open");
            }
        });

    },

    /* ---------------------------------------------------
     * Build dữ liệu cho combobox chọn thành viên đăng ký
     * Author: NVManh (29/04/2018)
     */
    buildDataForComboboxRegisterSelect: function (data) {
        cboRegisterSelect = detailForm.cboRegisterSelect;
        cboRegisterSelect.html('<option value="" disabled selected>--- Chọn người đăng ký ---</option>');
        // On success, 'data' contains a list of products.
        $.each(data, function (key, item) {
            // Add a list item for the product.
            var lastName = item.LastName === 'Mạnh' ? '<span style="color:red;">Mạnh</span>' : item.LastName,
                contactName = item.FirstName + ' ' + '<strong>' + lastName + '</strong>';
            $('<option>', {
                html: '<option>{0} ({1})</option>'.format(lastName, contactName),
                value: item.ContactId
            }).appendTo(cboRegisterSelect);
        });
    },

    /* ---------------------------------------------------
     * Xác nhận đăng ký tham dự
     * Author: NVManh (29/04/2018)
     */
    register: function () {
        var me = detailForm;
        var registerId = me.cboRegisterSelect.val(),
            eventId = me.masterId,
            numberAccompanying = me.numPerson.val();
        if (registerId && eventId) {
            commonJS.showMask($('body'));
            var formData = new FormData();
            // Thông tin liên hệ:
            var eventDetail = {
                ContactId: registerId,
                EventId: eventId,
                NumberAccompanying: numberAccompanying !== '' ? numberAccompanying : 0
            };

            formData.append('eventDetail', JSON.stringify(eventDetail));
            serviceAjax.post('/api/events/PostEventDetail', JSON.stringify(eventDetail), false, function (result) {
                if (result.Success) {
                    detailForm.loadData();
                }
                commonJS.hideMask($('body'));
            });
        }
        me.dialogRegister.dialog("close");
        detailForm.loadData();
    }
});

