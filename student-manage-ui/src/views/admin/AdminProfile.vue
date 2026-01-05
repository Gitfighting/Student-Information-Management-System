<template>
	<div class="admin-profile">
		<!-- 左侧导航 -->
		<aside class="sidebar">
			<h2 class="sidebar-title">管理员后台</h2>
			<nav>
				<ul>
					<li><router-link to="/admin/home">主页</router-link></li>
					<li><router-link to="/admin/students">学生信息</router-link></li>
					<li><router-link to="/admin/courses">课程信息</router-link></li>
					<li><router-link to="/admin/grades">学生成绩</router-link></li>
					<li><router-link to="/admin/logs">登录日志</router-link></li>
					<li><a href="javascript:;" @click="logout">退出</a></li>
				</ul>
			</nav>
		</aside>

		<!-- 右侧内容 -->
		<main class="content">
			<router-view></router-view>
		</main>
	</div>
</template>

<script setup>
import { useRouter } from 'vue-router'
import { useUserStore } from '@/store/userStore.js'

const router = useRouter()
const userStore = useUserStore()

const logout = () => {
	userStore.logout()
	router.push('/login/admin')  // 退出后返回管理员登录页
}
</script>

<style scoped>
.admin-profile {
	display: flex;
	width: 100vw;
	height: 100vh;
	background: linear-gradient(to right, #e0f7fa, #ffffff);
}

/* 左侧导航 */
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

.sidebar nav ul {
	list-style: none;
	padding: 0;
	margin: 0;
}

.sidebar nav ul li {
	margin-bottom: 18px;
}

.sidebar nav ul li a {
	display: block;
	font-size: 16px;
	font-weight: 500;
	padding: 10px 14px;
	border-radius: 8px;
	color: #000;
	text-decoration: none;
	transition: all 0.3s ease;
}

.sidebar nav ul li a:hover,
.sidebar nav ul li a.router-link-active {
	background: #e0f7fa;
	color: #00796b;
}

/* 右侧内容区 */
.content {
	flex: 1;
	padding: 0 0 0 10px;

	display: flex;
	justify-content: center;
	/* align-items: flex-start; */
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