<template>
  <div class="register-container">
    <el-card shadow="hover" title="学生注册" class="register-card">
      <el-form
        ref="registerFormRef"
        :model="registerForm"
        :rules="registerRules"
        label-width="100px"
        class="mt-4"
      >
        <!-- 学号（主键，非自增） -->
        <el-form-item label="学号" prop="stuId">
          <el-input
            v-model.number="registerForm.stuId"
            placeholder="请输入学号（数字）"
            prefix-icon="Key"
            type="number"
            clearable
          ></el-input>
        </el-form-item>

        <!-- 姓名 -->
        <el-form-item label="姓名" prop="stuName">
          <el-input
            v-model="registerForm.stuName"
            placeholder="请输入姓名"
            prefix-icon="User"
            clearable
          ></el-input>
        </el-form-item>

        <!-- 性别 -->
        <el-form-item label="性别" prop="stuGender">
          <el-select
            v-model="registerForm.stuGender"
            placeholder="请选择性别"
            prefix-icon="UserFilled"
          >
            <el-option label="男" value="男"></el-option>
            <el-option label="女" value="女"></el-option>
          </el-select>
        </el-form-item>

        <!-- 年龄 -->
        <el-form-item label="年龄" prop="stuAge">
          <el-input
            v-model.number="registerForm.stuAge"
            placeholder="请输入年龄（15-40）"
            prefix-icon="Calendar"
            type="number"
            clearable
          ></el-input>
        </el-form-item>

        <!-- 专业 -->
        <el-form-item label="专业" prop="stuMajor">
          <el-input
            v-model="registerForm.stuMajor"
            placeholder="请输入专业"
            prefix-icon="Book"
            clearable
          ></el-input>
        </el-form-item>

        <!-- 出生日期 -->
        <el-form-item label="出生日期" prop="stuBirth">
          <el-date-picker
            v-model="registerForm.stuBirth"
            type="date"
            placeholder="请选择出生日期"
            prefix-icon="CalendarFilled"
            value-format="YYYY-MM-DD"
          ></el-date-picker>
        </el-form-item>

        <!-- 邮箱 -->
        <el-form-item label="邮箱" prop="stuEmail">
          <el-input
            v-model="registerForm.stuEmail"
            placeholder="请输入邮箱"
            prefix-icon="Message"
            clearable
          ></el-input>
        </el-form-item>

        <!-- 电话 -->
        <el-form-item label="电话" prop="stuPhone">
          <el-input
            v-model="registerForm.stuPhone"
            placeholder="请输入手机号"
            prefix-icon="Phone"
            clearable
          ></el-input>
        </el-form-item>

        <!-- 密码 -->
        <el-form-item label="密码" prop="stuPwd">
          <el-input
            v-model="registerForm.stuPwd"
            type="password"
            placeholder="请输入密码（6-20位）"
            prefix-icon="Lock"
            clearable
            show-password
          ></el-input>
        </el-form-item>

        <!-- 注册/取消按钮 -->
        <el-form-item>
          <el-button
            type="primary"
            @click="handleRegister"
            class="register-btn"
          >
            注册
          </el-button>
          <el-button
            type="default"
            @click="$router.push('/')"
            class="cancel-btn"
          >
            取消（去登录）
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { studentRegister } from '@/api/studentApi'
import { ElMessage } from 'element-plus'

// 1. 表单数据与验证规则
const registerFormRef = ref(null)
const registerForm = reactive({
  stuId: 0,          // 学号（数字）
  stuName: '',       // 姓名
  stuGender: '',     // 性别
  stuAge: 0,         // 年龄
  stuMajor: '',      // 专业
  stuBirth: '',      // 出生日期（YYYY-MM-DD）
  stuEmail: '',      // 邮箱
  stuPhone: '',      // 电话
  stuPwd: ''         // 密码
})
// 表单验证规则（与后端BLL层校验逻辑一致）
const registerRules = reactive({
  stuId: [
    { required: true, message: '请输入学号', trigger: 'blur' },
    { type: 'number', message: '学号必须为数字', trigger: 'blur' },
    // { min: 10000, max: 999999, message: '学号长度7位', trigger: 'blur' }
  ],
  stuName: [
    { required: true, message: '请输入姓名', trigger: 'blur' },
    // { min: 2, max: 10, message: '姓名长度2-10位', trigger: 'blur' }
  ],
  stuGender: [
    { required: true, message: '请选择性别', trigger: 'change' }
  ],
  stuAge: [
    { required: true, message: '请输入年龄', trigger: 'blur' },
    { type: 'number', message: '年龄必须为数字', trigger: 'blur' },
    // { min: 15, max: 40, message: '年龄范围15-40岁', trigger: 'blur' }
  ],
  stuMajor: [
    { required: true, message: '请输入专业', trigger: 'blur' },
    { min: 2, max: 20, message: '专业长度2-20位', trigger: 'blur' }
  ],
  stuBirth: [
    { required: true, message: '请选择出生日期', trigger: 'change' }
  ],
  stuEmail: [
    { required: true, message: '请输入邮箱', trigger: 'blur' },
    { type: 'email', message: '邮箱格式不正确', trigger: 'blur' }
  ],
  stuPhone: [
    { required: true, message: '请输入手机号', trigger: 'blur' },
    { pattern: /^1[3-9]\d{9}$/, message: '手机号格式不正确', trigger: 'blur' }
  ],
  stuPwd: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    { min: 6, max: 20, message: '密码长度6-20位', trigger: 'blur' }
  ]
})

// 2. 依赖实例
const router = useRouter()

// 3. 注册处理
const handleRegister = async () => {
  try {
    // 1. 表单校验（验证失败会进入catch）
    await registerFormRef.value.validate()

    // 2. 验证通过，调用注册接口
    await studentRegister(registerForm)
    // 3. 注册成功，提示并跳转登录页
    ElMessage.success('注册成功！请登录')
    router.push('/login/student')
  } catch (error) {
    // 处理验证失败或接口调用失败的错误
    if (error instanceof Error) {
      console.error('操作失败：', error.message)
    } else {
      // 表单验证失败的错误（Element Plus 返回的验证信息）
      console.error('表单验证失败：', error)
      ElMessage.error('请检查表单填写是否正确')
    }
  }
}
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background-color: #f5f7fa;
}
.register-card {
  width: 500px;
  padding: 20px;
}
.register-btn {
  width: 45%;
  margin-right: 10px;
}
.cancel-btn {
  width: 45%;
}
</style>