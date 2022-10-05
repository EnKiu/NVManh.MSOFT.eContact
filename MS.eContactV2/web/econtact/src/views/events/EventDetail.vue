<template>
  <m-dialog title="Chi tiết sự kiện" @onClose="onClose">
    <template v-slot:content>
      <div class="registers">
        <div class="register__title">Danh sách đã đăng ký</div>
        <div class="register__list">
          <table class="table" border="0" width="100%">
            <thead>
              <tr>
                <th>#</th>
                <th>Họ và tên</th>
                <th>Hành động</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(contact, index) in registers" :key="index">
                <td>
                  {{ index + 1 }}
                </td>
                <td>
                  <div>{{ contact.FullName }}</div>
                  <div>(Đi kèm: {{ contact.NumberAccompanying }}) - <span class="show-note">Có ý kiến</span></div>
                </td>
                <td>
                  <div class="cell--action">
                    <div><button class="btn btn--table --color-red">Hủy đăng ký</button></div>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div class="event-detail">
        <div class="event__title">Thông tin sự kiện</div>
        <div class="event__content"></div>
      </div>
    </template>
    <template v-slot:footer> </template>
  </m-dialog>
</template>
<script>
export default {
  name: "EventDetail",
  components: {},
  emits: ["onCloseDetail"],
  props: ["eventItem"],
  created() {
    // Lấy danh sách đăng ký:
    this.api({
      url: "/api/v1/EventDetails/registers?eventId=" + this.eventItem.EventId,
      method: "GET",
    }).then((res) => {
      this.registers = res;
    });
  },
  methods: {
    onClose() {
      this.$emit("onCloseDetail");
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
.register__list {
  height: 250px;
  overflow-y: auto;
  box-sizing: border-box;
}

.show-note {
  margin-top: 4px;
  color: #3395ff;
  cursor: pointer;
}

.show-note:hover {
  text-decoration: underline;
}

.cell--action {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
</style>
