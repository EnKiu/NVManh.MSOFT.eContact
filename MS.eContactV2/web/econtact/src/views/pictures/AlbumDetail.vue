<template>
  <div class="album-dialog">
    <button class="close-album" @click="onCloseAlbumDetail">
      <i class="icofont-ui-close"></i>
    </button>
    <div class="album__title">{{ album.AlbumName }}</div>
    <div class="pictures">
      <div class="picture-item" v-for="(pic, index) in pictures" :key="index">
        <div class="picture__img" @click="onShowTranslation(index, pic)">
          <img :src="pic.UrlFullPath" alt="" />
        </div>
        <div class="picture__description">{{ pic.Description }}</div>
      </div>
    </div>
    <button id="btn-close" class="btn dialog__button--cancel" @click="onCloseAlbumDetail">
      <i class="icofont-close" @click="onCloseAlbumDetail"></i> Đóng lại
    </button>
  </div>
  <div v-if="showTransition" class="picture-transition">
    <div class="transition-container">
      <button class="close-album" @click="onCloseTransition">
        <i class="icofont-ui-close"></i>
      </button>
      <transition name="slide-fade">
        <!-- 			SRC comes from the array of images the :key is important for vue to believe its a 'new' DOM element and do the transition -->
        <img v-bind:src="urlPath" v-bind:key="currentIndex" />
      </transition>
      <div
        v-if="currentIndex > 0"
        class="transition-panel panel--left"
        @click="onPrevPicture"
      >
        <button class="btn-transition btn--prev">
          <i class="icofont-swoosh-left"></i>
        </button>
      </div>
      <div
        v-if="currentIndex < pictures.length - 1"
        class="transition-panel panel--right"
        @click="onNextPicture"
      >
        <button class="btn-transition btn--next">
          <i class="icofont-swoosh-right"></i>
        </button>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "AlbumDetail",
  components: {},
  emits: ["onCloseAlbumDetail"],
  props: ["album"],
  created() {
    this.loadPictures();
  },
  computed: {
    urlPath: function(){
        if(!this.pictures||this.pictures.length==0){
            return "";
        }else{
            return this.pictures[this.currentIndex].UrlFullPath;
        }
    }
  },
  methods: {
    loadPictures() {
      this.api({
        url: "/api/v1/Albums/" + this.album.AlbumId,
      }).then((res) => {
        console.log(res);
        this.pictures = res;
      });
    },
    onCloseAlbumDetail() {
      this.$emit("onCloseAlbumDetail");
    },
    onCloseTransition() {
      this.showTransition = false;
    },
    onShowTranslation(index, pic) {
      console.log(index);
      this.showTransition = true;
      this.pictureTransition = pic;
      this.currentIndex = index;
    },
    onNextPicture() {
      this.currentIndex++;
    },
    onPrevPicture() {
      this.currentIndex--;
    },
  },
  data() {
    return {
      pictures: [],
      pictureTransition: null,
      showTransition: false,
      currentIndex: null,
    };
  },
};
</script>
<style scoped>
.album-dialog {
  position: fixed;
  display: flex;
  flex-direction: column;
  top: 100px;
  left: 24px;
  right: 24px;
  bottom: 24px;
  background-color: #000;
  border-radius: 4px;
  /* max-height: calc(100vh - 100px); */
}
button.close-album {
  position: absolute;
  top: -10px;
  right: -10px;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  z-index: 2;
}
.album__title {
  color: #fff;
  font-weight: 700;
  font-size: 20px;
  padding: 24px 24px 0 24px;
}
.pictures {
  padding: 0 24px 24px 24px;
  overflow-y: auto;
  text-align: center;
}
.picture-item {
  max-width: 280px;
  float: left;
  padding: 4px;
  box-sizing: border-box;
}
.picture__img {
  width: 100%;
  float: left;
  margin: 5px;
  box-sizing: border-box;
}

.picture__img img {
  width: 100%;
  cursor: pointer;
}
.picture__img img:hover {
  opacity: 0.6;
}

#btn-close {
  position: absolute;
  bottom: 24px;
  right: 24px;
}

.picture-transition {
  position: fixed;
  top: 100px;
  left: 24px;
  right: 24px;
  bottom: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.transition-container {
  position: relative;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #000;
  border-radius: 4px;
  box-sizing: border-box;
}

.transition-container img {
  max-width: 100%;
  max-height: 100%;
  -webkit-transition: all 100ms ease;
  transition: all 100ms ease;
  box-sizing: border-box;
}
/* prefix with transition name */
.slide-fade-enter-active {
  opacity: 1;
  z-index: 10;
}

.slide-fade-leave-active {
  opacity: 1;
}

.slide-fade-enter,
.slide-fade-leave-to {
  opacity: 0;
}
.transition-panel {
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 70px;
  height: 100%;
  opacity: 0.4;
  z-index: 1;
}
.transition-panel:hover {
  background-color: #0000005f;
  opacity: 1;
}
.panel--left {
  left: 0;
}

.panel--right {
  right: 0;
}
.btn-transition {
  border-radius: 50%;
  width: 40px;
  height: 40px;
  font-size: 24px;
  cursor: pointer;
  border: unset;
  outline: none;
}

.btn-transition:hover {
  opacity: 1;
}
</style>
