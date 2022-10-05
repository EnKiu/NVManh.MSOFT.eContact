<template>
  <div class="container">
    <div class="toolbar"></div>
    <div class="event-list">
      <event-item
        v-for="(item, index) in events"
        :key="index"
        :item="item"
        @onRegister="onRegister(item)"
         @onShowList="onShowList(item)"
      ></event-item>
    </div>
    <event-register
      v-if="showRegister"
      @onClose="onCloseRegister"
      :eventRegister="eventRegister"
    ></event-register>
    <event-detail v-if="showDetail" @onCloseDetail="onCloseDetail" :eventItem="eventDetailSelected"></event-detail>
  </div>
</template>
<script>
import EventItem from "./EventItem.vue";
import EventRegister from "./EventRegister.vue";
import EventDetail from "./EventDetail.vue";
export default {
  name: "EventList",
  components: { EventItem, EventRegister, EventDetail },
  emits: [],
  props: [],
  created() {
    this.loadData();
  },
  methods: {
    onShowList(event){
      this.eventDetailSelected = event;
      this.showDetail = true;
    },
    onRegister(currentEvent) {
      this.showRegister = true;
      this.eventRegister = currentEvent;
    },
    onCloseRegister() {
      this.showRegister = false;
    },
    onCloseDetail() {
      this.showDetail = false;
    },
    loadData() {
      var baseUrl = process.env.VUE_APP_BASE_URL;
      fetch(baseUrl + "/api/v1/events")
        .then((res) => res.json())
        .then((res) => {
          this.events = res;
        })
        .catch((res) => {
          console.log(res);
        });
    },
  },
  data() {
    return {
      events: [],
      eventRegister: {},
      eventDetailSelected:null,
      showDetail: false,
      showRegister: false,
    };
  },
};
</script>
<style scoped>
.container {
  padding: 24px;
}
</style>
