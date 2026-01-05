import { defineStore } from 'pinia'
import { useRouter } from 'vue-router'
import { studentLogin } from '@/api/studentApi'
import { adminLogin } from '@/api/adminApi'

export const useUserStore = defineStore('user', {
	state: () => ({
		isLogin: false,          // 是否登录
		userType: '',            // 'student' 或 'admin'
		userInfo: {}             // 学生或管理员信息
	}),
	actions: {
		// 学生登录
		async loginStudent(loginData) {
			const res = await studentLogin(loginData)
			if (res.code === 2) {
				this.isLogin = true
				this.userType = 'student'
				this.userInfo = res.data
				localStorage.setItem('user', JSON.stringify({
					isLogin: true,
					userType: 'student',
					userInfo: res.data
				}))
				return res
			} else {
				throw new Error(res.message || '学生登录失败')
			}
		},

		// 管理员登录
		async loginAdmin(loginData) {
			const res = await adminLogin(loginData)
			if (res.code === 2) {
				this.isLogin = true
				this.userType = 'admin'
				this.userInfo = res.data
				localStorage.setItem('user', JSON.stringify({
					isLogin: true,
					userType: 'admin',
					userInfo: res.data
				}))
				return res
			} else {
				throw new Error(res.message || '管理员登录失败')
			}
		},

		// 退出登录
		logout() {
			const router = useRouter()
			this.isLogin = false
			this.userType = ''
			this.userInfo = {}
			localStorage.removeItem('user')
			router.push('/') // 跳回选择登录页
		},

		// 刷新页面时恢复状态
		restoreState() {
			const data = localStorage.getItem('user')
			if (data) {
				const { isLogin, userType, userInfo } = JSON.parse(data)
				this.isLogin = isLogin
				this.userType = userType
				this.userInfo = userInfo
			}
		}
	}
})
