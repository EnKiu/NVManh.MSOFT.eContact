<template>
  <m-dialog title="Danh sách đăng ký tham dự" @onClose="onClose">
    <template v-slot:content>
      <div class="registers">
        <div class="register__header">
          <div class="register_number">
            Tổng số có
            <b>{{ eventDetail.TotalMember + eventDetail.TotalAccompanying }}</b>
            người tham dự.
          </div>
          <div class="register_number">
            - Có {{ eventDetail.TotalMember }} thành viên.
          </div>
          <div class="register_number">
            - Có {{ eventDetail.TotalAccompanying }} khách đi kèm.
          </div>
        </div>
        <div class="register__list">
          <m-table
            ref="tbListDocument"
            :data="registers"
            empty-text="Không có dữ liệu"
            width="100%"
            height="100%"
          >
            <!-- <m-column prop="FullName" type="expand">
              <template #default="scope">
                <div class="cell__row">{{ scope.row.FullName }}</div>
                <div class="cell__row --mini">
                  (Đi kèm: {{ scope.row.NumberAccompanying }}) -
                  <span class="show-note">Có ý kiến</span>
                </div>
              </template>
            </m-column> -->
            <el-table-column label="#" type="index" width="30" />

            <m-column prop="FullName" label="Họ và tên">
              <template #default="scope">
                <div class="cell__row">{{ scope.row.FullName }}</div>
                <div class="cell__row --mini">
                  <div v-if="!scope.row.NotYetPaid" style="color: #00b87a">
                    + Nộp quỹ:
                    {{ commonJs.formatMoney(scope.row.Amount) }}
                    <button
                      v-if="!expireTime && isAdmin"
                      id="btnRemoveSpend"
                      title="Hủy nộp tiền"
                      @click="onRemoveSpends(scope.row)"
                    >
                      <i class="icofont-ui-close"></i>
                    </button>
                  </div>
                  <div v-else style="color: #ff0000">
                    - Chưa đóng quỹ.
                    <button
                      v-if="isAdmin"
                      id="btnAddMoney"
                      title="Nộp tiền"
                      @click="onAddMoney(scope.row)"
                    >
                      <i class="icofont-ui-add"></i>
                    </button>
                  </div>
                  <div v-if="scope.row.NumberAccompanying > 0">
                    - Đi kèm: {{ scope.row.NumberAccompanying }}
                  </div>
                  <div class="flex" v-if="scope.row.Note">
                    <span>- Ý kiến:</span>
                    <div
                      class="show-note"
                      :title="scope.row.Note"
                      @click="
                        commentSelected = scope.row.Note;
                        fullNameComment = scope.row.FullName;
                      "
                    >
                      {{ scope.row.Note }}
                    </div>
                  </div>
                </div>
              </template>
            </m-column>
            <m-column v-if="isAdmin" label="Hủy đăng ký" width="70">
              <template #header>
                <button
                  id="btn-add-register"
                  class="btn btn--default"
                  @click="onRegister"
                >
                  <i class="icofont-ui-add"></i> Thêm đăng ký
                </button>
              </template>
              <template #default="scope">
                <div class="button-column">
                  <button
                    class="btn--table-mini --color-red"
                    :title="scope.row.FullName"
                    @click="onCancelRegister(scope.row)"
                  >
                    <i class="icofont-ui-delete"></i> <span>Xóa</span>
                  </button>
                  <button
                    class="btn--table-mini --color-edit"
                    :title="scope.row.FullName"
                    @click="onUpdateRegister(scope.row)"
                  >
                    <i class="icofont-ui-edit"></i> <span>Sửa</span>
                  </button>
                </div>
              </template>
            </m-column>
          </m-table>
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--default dialog__button--cancel" @click="onClose">
        <i class="icofont-ui-close"></i> Đóng
      </button>
    </template>
  </m-dialog>
  <event-comment
    v-if="commentSelected != null"
    v-model:comment="commentSelected"
    :fullName="fullNameComment"
  ></event-comment>
  <m-dialog title="Nộp tiền" v-if="showAddMoneyForm" @onClose="showAddMoneyForm = false">
    <template v-slot:content>
      <div class="add-money">
        <m-input
          label="Thành viên nộp tiền"
          :disabled="true"
          v-model="registerForUpdate.FullName"
        ></m-input>
        <m-input
          label="Số tiền"
          :isFocus="true"
          :required="true"
          class="mg-bottom-0"
          type="number"
          v-model="registerForUpdate.SpendsTotal"
        ></m-input>
        <div style="text-align: right">
          <b>{{ moneyFormat }}</b>
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--default" @click="onSaveEventDetail">
        <i class="icofont-ui-check"></i> Nộp tiền
      </button>
    </template>
  </m-dialog>
</template>
<script>
import EventComment from "./EventComment.vue";
export default {
  name: "EventDetail",
  components: { EventComment },
  emits: [
    "onCloseDetail",
    "onRegisterFromDetail",
    "afterCancelRegisterSuccess",
    "onUpdateRegister",
  ],
  props: ["eventItem", "isAdmin"],
  created() {
    var registerDate = new Date(this.eventItem.ExpireRegisterDate);
    if (registerDate && registerDate < new Date()) {
      this.expireTime = true;
    }
    this.eventDetail = this.eventItem;
    this.loadRegisters();
  },
  computed: {
    moneyFormat: function () {
      return this.commonJs.formatMoney(this.registerForUpdate.SpendsTotal);
    },
  },
  methods: {
    loadRegisters() {
      // Lấy danh sách đăng ký:
      this.api({
        url: "/api/v1/EventDetails/registers?eventId=" + this.eventItem.EventId,
        method: "GET",
      }).then((res) => {
        this.registers = res;
      });
    },
    onAddMoney(register) {
      this.registerForUpdate = register;
      this.registerForUpdate.SpendsTotal = this.eventItem.Spends;
      this.showAddMoneyForm = true;
    },
    onRemoveSpends(register) {
      this.commonJs.showConfirm(
        `Bạn chắc chắn muốn xóa <b>${register.LastName}</b> khỏi danh sách đã nộp tiền?`,
        () => {
          this.registerForUpdate = register;
          this.registerForUpdate.SpendsTotal = 0;
          this.onSaveEventDetail();
        }
      );
    },
    onUpdateRegister(register) {
      this.$emit("onUpdateRegister", register, this.eventItem);
    },
    onSaveEventDetail() {
      this.api({
        url: "/api/v1/EventDetails/" + this.eventItem.EventId,
        method: "PUT",
        data: this.registerForUpdate,
      }).then(() => {
        this.showAddMoneyForm = false;
        this.loadRegisters();
      });
    },
    onClose() {
      this.$emit("onCloseDetail");
    },
    onRegister() {
      this.$emit("onCloseDetail");
      this.$emit("onRegisterFromDetail", this.eventItem);
    },
    onCancelRegister(register) {
      var me = this;
      this.commonJs.showConfirm(
        `Bạn có chắc chắn muốn hủy đăng ký ${register.FullName} tham gia sự kiện này?`,
        () => {
          this.api({
            url: `/api/v1/EventDetails/${register.EventDetailId}`,
            method: "DELETE",
          }).then((res) => {
            console.log(res);
            this.eventDetail.TotalAccompanying -= register.NumberAccompanying;
            this.eventDetail.TotalMember -= 1;
            this.loadRegisters();
            me.$emit("afterCancelRegisterSuccess", register);
          });
        }
      );
    },
  },
  data() {
    return {
      registers: [],
      eventDetail: {},
      registerForUpdate: {},
      showAddMoneyForm: false,
      commentSelected: null,
      fullNameComment: null,
      expireTime: false,
    };
  },
};
</script>
<style scoped>
div.add-money .input-wrapper:first-child {
  margin-top: 0px;
}

.mg-bottom-0 {
  margin-bottom: 4px !important;
}
div.add-money .input-wrapper:last-child {
  margin-bottom: 0px;
}
.register__header {
  border: 1px solid #dedede;
  border-radius: 4px;
  padding: 10px;
  margin-bottom: 10px;
}
.register__list {
  max-width: 400px;
  height: calc(100vh - 240px);
  overflow-y: auto;
  box-sizing: border-box;
}

.show-note {
  color: #3395ff;
  cursor: pointer;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 150px;
  margin-left: 4px;
}

.show-note:hover {
  text-decoration: underline;
}

.btn--table {
  height: 30px;
}
.cell--action {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.--mini {
  font-size: 11px;
  background-color: #cdddf7;
  padding: 2px;
}

.el-table__header button {
  height: 30px !important;
}

.el-table__row .cell {
  padding: 10px 0 !important;
}
.button-column {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  padding: 0 4px;
}
.btn--table-mini {
  background: none;
  font-size: 16px;
  width: 20px;
  text-align: center;
  border: unset;
  padding: 0;
  cursor: pointer;
}

.btn--table-mini + .btn--table-mini {
  margin-top: 10px;
}
#btnAddMoney,
#btnRemoveSpend {
  color: #00b87a;
  border: unset;
  background: none;
  font-size: 13px;
  cursor: pointer;
}

#btnRemoveSpend {
  color: #ff0000;
}

#btn-add-register {
  position: absolute;
  right: 0;
  top: 4px;
}
</style>
