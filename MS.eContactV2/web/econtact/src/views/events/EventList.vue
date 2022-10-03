<template>
  <div class="container">
    <div class="toolbar"></div>
    <div class="event-list">
      <event-item v-for="(item, index) in events" :key="index" :item="item"></event-item>
    </div>
    <event-register></event-register>
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
      showDetail: false,
    };
  },
};
</script>
<style scoped>
.container {
  padding: 24px;
}
</style>