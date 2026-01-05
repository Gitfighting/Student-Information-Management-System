<template>
  <div class="login-container">
    <div class="login-card">
      <h1 class="system-title">学生信息管理系统</h1>
      <h2 class="title">学生登录</h2>

      <el-form :model="loginForm" :rules="loginRules" ref="loginFormRef" class="login-form">
        <!-- 用户名输入 -->
        <el-form-item prop="stuName">
          <el-input v-model="loginForm.stuName" placeholder="请输入学号/用户名" size="large" :prefix-icon="User" clearable />
        </el-form-item>

        <!-- 密码输入 -->
        <el-form-item prop="stuPwd">
          <el-input v-model="loginForm.stuPwd" type="password" placeholder="请输入密码" size="large" :prefix-icon="Lock"
            show-password clearable />
        </el-form-item>

        <!-- 验证码输入 -->
        <el-form-item prop="captcha">
          <div class="captcha-row">
            <el-input v-model="loginForm.captcha" placeholder="请输入验证码" size="large" :prefix-icon="Key"
              class="captcha-input" maxlength="4" />
            <VerifyCode class="verify-code" v-model="generatedCaptcha" />
          </div>
        </el-form-item>

        <!-- 按钮组 -->
        <div class="btn-group">
          <el-button size="large" @click="$router.push('/')" plain class="custom-btn">
            取消
          </el-button>
          <el-button size="large" @click="$router.push('/register')" plain class="custom-btn register-btn">
            注册
          </el-button>
          <el-button type="primary" size="large" @click="handleLogin" class="custom-btn" :loading="loading">
            {{ loading ? '登录中...' : '登录' }}
          </el-button>
        </div>
      </el-form>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { User, Lock, Key } from '@element-plus/icons-vue'
import VerifyCode from '../components/common/VerifyCode.vue'
import { studentLogin } from '@/api/studentApi'
import { useUserStore } from '@/store/userStore'  // 引入
const router = useRouter()
const userStore = useUserStore()   // 获取 store 实例
// 响应式数据
const loginFormRef = ref()
const loading = ref(false)

const loginForm = reactive({
  stuName: '',
  stuPwd: '',
  captcha: ''
})

// 存储生成的验证码（由 VerifyCode 组件 emit 出来）
const generatedCaptcha = ref('')

// 表单验证规则
const loginRules = reactive({

  stuName: [
    { required: true, message: '请输入学号或用户名', trigger: 'blur' }
  ],
  stuPwd: [
    { required: true, message: '请输入密码', trigger: 'blur' }
  ],
  captcha: [
    { required: true, message: '请输入验证码', trigger: 'blur' }
  ]
})

// 处理登录
const handleLogin = async () => {
  // 使用 Promise 方式校验表单（Element Plus 支持 validate 返回 Promise）
  try {
    await loginFormRef.value.validate()
  } catch (err) {
    ElMessage.error('请完善表单信息')
    return
  }

  // 1. 校验验证码（前端判断）
  if (loginForm.captcha.toLowerCase() !== generatedCaptcha.value.toLowerCase()) {
    ElMessage.error('验证码错误')
    return
  }

  // 登录逻辑
  loading.value = true
  try {
    // 调用封装的登录接口
    const res = await studentLogin(loginForm)
    console.log("res：", res);           // 打印整个响应
    console.log("res.data:", res.data);      // 打印真正的数据

    // 根据后端返回的 code 处理不同情况
    if (res.code === 2) {
      ElMessage.success(res.data.message || '登录成功')

      await userStore.loginStudent({
        stuName: loginForm.stuName,
        stuPwd: loginForm.stuPwd
      })
      // 跳转到学生 dashboard 页面
      router.push('/student/home')
    } else {
      // 显示后端返回的具体错误信息（用户不存在/密码错误等）
      ElMessage.error(res.data.message || '登录失败，请检查账号密码')
    }
  } catch (error) {
    console.error("登录异常：", error);
    // 捕获网络错误或服务器异常
    ElMessage.error('登录失败，请稍后重试')
  } finally {
    // 无论成功失败，都关闭加载状态
    loading.value = false
  }
}
</script>

<style scoped>
.login-container {
  width: 100vw;
  height: 100vh;
  background: linear-gradient(to right, #e0f7fa, #ffffff);
  display: flex;
  justify-content: center;
  align-items: center;
}

.login-card {
  text-align: center;
  padding: 40px 60px;
  border-radius: 16px;
  background: #ffffffcc;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
  width: 100%;
  max-width: 480px;
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

.login-form {
  margin-top: 20px;
}

.captcha-row {
  display: flex;
  gap: 10px;
  align-items: center;
}

.captcha-input {
  flex: 1;
}

.verify-code {
  flex-shrink: 0;
}

.btn-group {
  display: flex;
  justify-content: center;
  gap: 20px;
  margin-top: 20px;
}

.custom-btn {
  font-size: 16px;
  font-weight: 500;
  padding: 12px 24px;
  border-radius: 10px;
  border: 2px solid #000;
  background-color: #fff;
  color: #000;
  min-width: 100px;
  flex: 1;
}

.custom-btn:focus,
.custom-btn:hover {
  background-color: #f5f5f5;
}

/* 注册按钮样式 */
.register-btn {
  background-color: #f0f0f0;
  border-color: #666;
}

.register-btn:hover {
  background-color: #e0e0e0;
}

/* 为登录按钮添加特殊样式 */
.btn-group .custom-btn:last-child {
  background-color: #000;
  color: #fff;
  border-color: #000;
}

.btn-group .custom-btn:last-child:hover {
  background-color: #333;
  border-color: #333;
}

/* 响应式设计 */
@media (max-width: 600px) {
  .login-card {
    padding: 30px 20px;
    margin: 20px;
  }

  .btn-group {
    flex-direction: column;
    gap: 15px;
  }

  .captcha-row {
    flex-direction: column;
  }

  .verify-code {
    width: 100%;
  }

  .custom-btn {
    min-width: auto;
  }
}
</style>