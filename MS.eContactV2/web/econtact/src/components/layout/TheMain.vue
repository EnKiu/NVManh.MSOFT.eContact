<template>
    <div class="main">
        <router-view></router-view>
    </div>
</template>
<script>
export default {
    
}
</script>

<style scoped>
 .main{
    margin-top: 80px;
    width: 100%;
    height:calc(100vh - 80px);
    display: flex;
    justify-content: center;
    padding: 24px;
    box-sizing: border-box;
    overflow-y: auto;
    background-color: #fff;
    z-index: 99;
 }
</style>