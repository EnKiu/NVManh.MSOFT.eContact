<template>
    <div class="album">
       <div class="album__toolbar">
        <button class="btn btn--default" id="btn-add" @click="onAddNewAlbum">Thêm mơi</button>
       </div>
       <div class="album__list"></div>
    </div>
    <picture-detail></picture-detail>
</template>
<script>
import PictureDetail from './PictureDetail.vue'
export default {
    name:"PictureList",
    components:{PictureDetail},
    props:[],
    emits:[],
    methods: {
        onAddNewAlbum(){

        }
    },
    data() {
        return {
            showAddNew: false
        }
    },
}
</script>
<style scoped>

</style>