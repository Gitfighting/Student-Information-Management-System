import axios from 'axios'
import { ElMessage } from 'element-plus'

// 创建Axios实例（对接后端API基础地址）
const request = axios.create({
	baseURL: 'http://localhost:5000/api', // 后端API地址（需与后端IIS/VS配置一致）
	timeout: 10000, // 超时时间10秒
	headers: {
		'Content-Type': 'application/json' // 后端接收JSON格式数据
	}
})

// 1. 请求拦截器：添加登录状态（如后续有Token可在此处添加）
request.interceptors.request.use(
	(config) => {
		// 自动注入 isAdmin 参数（从 localStorage 获取用户类型）
		const userStr = localStorage.getItem('user')
		let isAdmin = false

		if (userStr) {
			const user = JSON.parse(userStr)
			isAdmin = user.userType === 'admin'
		}

		// 为 POST/PUT 请求处理 isAdmin
		if (config.method === 'post' || config.method === 'put') {
			// 检查 URL 是否需要管理员权限（add/update/delete 等操作）
			const needsAdminCheck = /\/(add|update|delete|query|getAll)/i.test(config.url)

			if (needsAdminCheck) {
				// 对于 POST 请求，将 isAdmin 添加到 URL 参数
				const separator = config.url.includes('?') ? '&' : '?'
				config.url = `${config.url}${separator}isAdmin=${isAdmin}`
			}
		}

		// 为 GET/DELETE 请求添加 isAdmin 参数
		if (config.method === 'get' || config.method === 'delete') {
			const separator = config.url.includes('?') ? '&' : '?'
			config.url = `${config.url}${separator}isAdmin=${isAdmin}`
		}

		console.log("🚀 请求:", config.method?.toUpperCase(), config.url, "data:", config.data);
		return config
	},
	(error) => {
		console.error("❌ 请求错误:", error);
		ElMessage.error('请求发送失败，请检查网络')
		return Promise.reject(error)
	}
)

// 2. 响应拦截器：统一处理后端ResultVO格式
request.interceptors.response.use(
	(response) => {
		const res = response.data
		// 后端状态码：0=失败，1=业务失败，2=成功（与BLL层ResultVO一致）
		if (res.code !== 2) {
			// 失败时弹窗提示（如“用户不存在”“密码错误”）
			ElMessage.error(res.message || '操作失败')
			return Promise.reject(res.message)
		}
		return res // 成功时返回完整响应数据（含data）
	},
	(error) => {
		if (error.response) {
			console.error("❌ 响应错误:", error.response.status, error.response.data);
		} else {
			console.error("❌ 网络连接超时:", error.message);
		}
		// 网络错误/后端500错误处理
		ElMessage.error('服务器异常，请联系管理员')
		return Promise.reject(error)
	}
)

export default request