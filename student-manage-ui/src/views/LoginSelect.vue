<template>
  <div class="select-container">
    <div class="select-card">
      <h1 class="system-title">学生信息管理系统</h1>
      <h2 class="title">请选择登录身份</h2>
      <div class="btn-group">
        <el-button class="custom-btn" size="large" @click="$router.push('/login/student')">
          学生登录
        </el-button>
        <el-button class="custom-btn" size="large" @click="$router.push('/login/admin')">
          管理员登录
        </el-button>
      </div>
    </div>
  </div>
</template>

<script setup>
</script>

<style scoped>
.select-container {
  width: 100vw;
  height: 100vh;
  background: linear-gradient(to right, #e0f7fa, #ffffff);
  display: flex;
  justify-content: center;
  align-items: center;
}

.select-card {
  text-align: center;
  padding: 40px 60px;
  border-radius: 16px;
  background: #ffffffcc;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
}

.system-title {
  font-size: 28px;
  font-weight: bold;
  color: #333;
  margin-bottom: 20px;
}

.title {
  font-size: 20px;
  font-weight: 500;
  color: #555;
  margin-bottom: 30px;
}

.btn-group {
  display: flex;
  justify-content: center;
  gap: 30px;
}

.custom-btn {
  font-size: 18px;
  font-weight: 500;
  color: #000 !important;
  padding: 12px 32px;
  border-radius: 10px;
  border: 2px solid #000;
  background-color: #fff !important; /* 背景白色 */
}
</style>
