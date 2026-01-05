<script setup>

</script>

<template>
  <!-- 路由出口：所有页面（登录/注册/个人信息）将通过路由渲染到这里 -->
  <router-view />
</template>

<style >

/* 全局去掉默认边距 */
html, body, #app {
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
}

/* 让 router-view 占满整个屏幕 */
#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

</style>
