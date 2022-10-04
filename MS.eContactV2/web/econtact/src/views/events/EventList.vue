<template>
  <div class="container">
    <div class="toolbar"></div>
    <div class="event-list">
      <event-item v-for="(item, index) in events" :key="index" :item="item" @onRegister="onRegister(item)"></event-item>
    </div>
    <event-register v-if="showRegister" @onClose="onCloseRegister" :eventRegister="eventRegister"></event-register>
  </div>
</template>
<script>
    import EventItem from './EventItem.vue'
    import EventRegister from './EventRegister.vue'
export default {
  name: "EventList",
  components: {EventItem,EventRegister},
  emits: [],
  props: [],
  created() {
    this.loadData();
  },
  methods: {
    onRegister(currentEvent){
      this.showRegister = true;
      this.eventRegister = currentEvent;
    },
    onCloseRegister(){
      this.showRegister = false;
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
      eventRegister:{},
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