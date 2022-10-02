<template>
  <the-header></the-header>
  <the-main></the-main>
</template>

<script>
import TheHeader from './components/layout/TheHeader.vue'
import TheMain from './components/layout/TheMain.vue'
export default {
  name: 'App',
  components: {
    TheHeader,TheMain
  }
}
</script>

<style>
@import url(./styles/main.css);
</style>
