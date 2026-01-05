<template>
	<div class="profile-card">
		<!-- 左侧头像 -->
		<div class="avatar-area">
			<el-avatar :size="100" :src="avatarImg" />
			<h3 class="name">{{ studentInfo.stuName }}</h3>
			<p class="id">学号: {{ studentInfo.stuId }}</p>
		</div>

		<!-- 右侧信息 -->
		<div class="info-area">
			<h2>个人信息</h2>
			<el-divider />

			<div class="info-row">
				<strong>性别：</strong>
				<span>{{ studentInfo.stuGender }}</span>
			</div>
			<div class="info-row">
				<strong>年龄：</strong>
				<span>{{ studentInfo.stuAge }}</span>
			</div>
			<div class="info-row">
				<strong>专业：</strong>
				<span>{{ studentInfo.stuMajor }}</span>
			</div>
			<div class="info-row">
				<strong>出生日期：</strong>
				<span>{{ formatDate(studentInfo.stuBirth) }}</span>
			</div>
			<div class="info-row">
				<strong>邮箱：</strong>
				<span>{{ studentInfo.stuEmail }}</span>
			</div>
			<div class="info-row">
				<strong>电话：</strong>
				<span>{{ studentInfo.stuPhone }}</span>
			</div>
		</div>
	</div>
</template>

<script setup>
import { useUserStore } from '@/store/userStore.js'
import avatarImg from '@/assets/basketball.jpeg'  // 导入本地图片
const userStore = useUserStore()
const studentInfo = userStore.userInfo

function formatDate(dateStr) {
	if (!dateStr) return ''
	return dateStr.split('T')[0]
}
</script>

<style scoped>
.profile-card {
	display: flex;
	align-items: flex-start;
	background: #fff;
	padding: 30px;
	border-radius: 20px;
	box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
	max-width: 900px;
	margin: auto;
	gap: 40px;
}

.avatar-area {
	display: flex;
	flex-direction: column;
	align-items: center;
	min-width: 180px;
}

.avatar-area .name {
	margin-top: 15px;
	font-size: 20px;
	font-weight: bold;
}

.avatar-area .id {
	color: #666;
	font-size: 14px;
	margin-top: 5px;
}

.info-area {
	flex: 1;
}

.info-area h2 {
	font-size: 22px;
	font-weight: bold;
	margin-bottom: 15px;
}

.info-row {
	display: flex;
	justify-content: flex-start;
	align-items: center;
	font-size: 16px;
	margin: 10px 0;
	padding: 6px 0;
	border-bottom: 1px solid #f0f0f0;
}

.info-row strong {
	width: 100px;
	color: #333;
}

.info-row span {
	color: #555;
}
</style>
