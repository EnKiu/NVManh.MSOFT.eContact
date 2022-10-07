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
            @row-dblclick="rowDblClick"
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
                <div
                  v-if="scope.row.NumberAccompanying > 0 || scope.row.Note"
                  class="cell__row --mini flex"
                >
                  <span v-if="scope.row.NumberAccompanying > 0"
                    >(Đi kèm: {{ scope.row.NumberAccompanying }})</span
                  >
                  <div class="flex" v-if="scope.row.Note">
                    <span>- Ý kiến:</span>
                    <div class="show-note" :title="scope.row.Note">
                      {{ scope.row.Note }}
                    </div>
                  </div>
                </div>
              </template>
            </m-column>
            <m-column label="Hủy đăng ký" width="120">
              <template #header>
                <button class="btn btn--default">Đăng ký thêm</button>
              </template>
              <template #default="scope">
                <button
                  class="btn btn--table --color-red"
                  :title="scope.row.FullName"
                  @click="onCancelRegister(scope.row)"
                >
                  Hủy đăng ký
                </button>
              </template>
            </m-column>
          </m-table>
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--default" @click="onClose">
        <i class="icofont-ui-close"></i> Đóng
      </button>
    </template>
  </m-dialog>
</template>
<script>
export default {
  name: "EventDetail",
  components: {},
  emits: ["onCloseDetail"],
  props: ["eventItem"],
  created() {
    this.eventDetail = this.eventItem;
    this.loadRegisters();
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
    onClose() {
      this.$emit("onCloseDetail");
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
            this.eventDetail.TotalAccompanying-=register.NumberAccompanying;
            this.eventDetail.TotalMember-=1;
            this.loadRegisters();
            me.$emit("@afterCancelRegisterSuccess", register);
          });
        }
      );
    },
  },
  data() {
    return {
      registers: [],
      eventDetail: {},
    };
  },
};
</script>
<style scoped>
.register__header {
  border: 1px solid #dedede;
  border-radius: 4px;
  padding: 10px;
  margin-bottom: 10px;
}
.register__list {
  max-width: 400px;
  height: 250px;
  overflow-y: auto;
  box-sizing: border-box;
}

.show-note {
  color: #3395ff;
  cursor: pointer;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100px;
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
}

.el-table__header button {
  height: 30px !important;
}
</style>
