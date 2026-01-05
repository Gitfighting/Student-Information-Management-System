<template>
  <div class="profile-container">
    <!-- 左侧导航栏 -->
    <div class="sidebar">
      <h2 class="sidebar-title">学生信息管理系统</h2>
      <ul class="menu">
        <li :class="{ active: activeTab === 'home' }" @click="goTab('home')">首页</li>
        <li :class="{ active: activeTab === 'info' }" @click="goTab('info')">个人信息</li>
        <li :class="{ active: activeTab === 'grades' }" @click="goTab('grades')">个人成绩</li>
        <li @click="logout">退出</li>
      </ul>
    </div>

    <!-- 右侧内容区 -->
    <div class="content">
      <router-view />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/store/userStore.js'

const router = useRouter()
const userStore = useUserStore()

const activeTab = ref('home')

// 点击切换
function goTab(tab) {
  activeTab.value = tab
  router.push(`/student/${tab}`)
}

// 退出系统
function logout() {
  userStore.$reset()
  router.push('/login/student')
}
</script>

<style scoped>
.profile-container {
  width: 100vw;
  height: 100vh;
  display: flex;
  background: linear-gradient(to right, #e0f7fa, #ffffff);
}

.sidebar {
width: 240px;
  background: #ffffffcc;
  box-shadow: 4px 0 12px rgba(0, 0, 0, 0.1);
  padding: 30px 20px;
  display: flex;
  flex-direction: column;
  border-radius: 0 16px 16px 0;
}

.sidebar-title {
  font-size: 22px;
  font-weight: bold;
  color: #333;
  margin-bottom: 30px;
  text-align: center;
}

.menu {
  list-style: none;
  padding: 0;
  margin: 0;
}

.menu li {
  font-size: 18px;
  font-weight: 500;
  color: #333;
  padding: 12px 16px;
  margin-bottom: 12px;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s;
  text-align: center;
}

.menu li:hover {
  background: #e0f7fa;
}

.menu li.active {
  background: #000;
  color: #fff;
}

.content {
  flex: 1;
    padding: 0 0 0 10px;

    display: flex;
    justify-content: center;
}

.content-card {
  width: 100%;
  height: 100%;
  background: #ffffffcc;
  border-radius: 16px;
  padding: 30px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
}

</style>
