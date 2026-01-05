import { createRouter, createWebHistory } from 'vue-router'
import { useUserStore } from '@/store/userStore.js'


// 路由规则（分公开路由和需要登录的路由）
const routes = [
	// 公开路由（未登录可访问）
	{
		path: '/',
		component: () => import('@/views/LoginSelect.vue'),
		name: 'LoginSelect'
	}, // 入口页（选择学生/管理员登录）

	{
		path: '/login/student',
		component: () => import('@/views/StudentLogin.vue'),
		name: 'StudentLogin'
	}, // 学生登录页

	{
		path: '/login/admin',
		component: () => import('@/views/AdminLogin.vue'),
		name: 'AdminLogin'
	}, // 管理员登录页

	{
		path: '/register',
		component: () => import('@/views/Register.vue'),
		name: 'Register'
	},

	// 学生私有路由（需登录+学生权限）
	{
		path: '/student',
		component: () => import('@/views/student/StudentProfile.vue'),
		name: 'StudentProfile',
		meta: {
			requiresAuth: true, // 标记需要登录
			userType: 'student' // 标记需要学生权限
		},
		redirect: '/student/home',
		children: [
			{ path: 'home', component: () => import('@/views/student/Home.vue') },
			{ path: 'info', component: () => import('@/views/student/StudentInfo.vue') },
			{ path: 'grades', component: () => import('@/views/student/StudentGrades.vue') }
		]

	},

	// 管理员私有路由（需登录+管理员权限）
	{
		path: '/admin',
		component: () => import('@/views/admin/AdminProfile.vue'),
		name: 'AdminProfile',
		meta: {
			requiresAuth: true,
			userType: 'admin'
		},
		redirect: '/admin/home',// 默认跳到主页
		children: [
			{ path: 'home', component: () => import('@/views/admin/Home.vue') },
			{ path: 'students', component: () => import('@/views/admin/StudentList.vue') },
			{ path: 'courses', component: () => import('@/views/admin/CourseList.vue') },
			{ path: 'grades', component: () => import('@/views/admin/GradeList.vue') },
			{ path: 'logs', component: () => import('@/views/admin/LogList.vue') },
		]
	}
]


const router = createRouter({
	history: createWebHistory(), // 路由模式（无#）
	routes
})

// 路由前置守卫：未登录拦截（保护私有路由）
router.beforeEach((to, from, next) => {

	const userStore = useUserStore()

	// 1. 页面刷新时先恢复状态（从本地存储）
	if (!userStore.isLogin) {
		userStore.restoreState()
	}

	// 2. 处理需要登录的路由
	if (to.meta.requiresAuth) {
		if (userStore.isLogin && userStore.userType === to.meta.userType) {
			// 已登录且权限匹配，允许访问
			next()
		} else {
			// 未登录或权限不匹配，跳转登录页
			next('/')
		}
	} else {
		// 公开路由，直接访问
		next()
	}
})

export default router