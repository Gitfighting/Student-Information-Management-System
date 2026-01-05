<template>
	<div class="student-grades">
		<!-- 区域 1：单科成绩 -->
		<el-card class="grade-card" shadow="hover">
			<template #header>
				<div class="card-header">
					<span>单科成绩</span>
					<span class="summary">通过数：{{ passCount }}</span>
				</div>
			</template>

			<el-table :data="singleScores" style="width: 100%">
				<el-table-column prop="stuId" label="学号" width="120" />
				<el-table-column prop="courseId" label="课程编号" width="150" />
				<el-table-column prop="score" label="课程成绩" width="120" />
				<el-table-column prop="courseCredit" label="学分" width="100" />
				<el-table-column prop="isPass" label="是否通过" width="120">
					<template #default="scope">
						<el-tag :type="scope.row.isPass ? 'success' : 'danger'">
							{{ scope.row.isPass ? '通过' : '未通过' }}
						</el-tag>
					</template>
				</el-table-column>
			</el-table>
		</el-card>

		<!-- 区域 2：科目平均成绩 -->
		<el-card class="grade-card" shadow="hover">
			<template #header>
				<div class="card-header">
					<span>科目成绩</span>
					<span class="summary">平均成绩：{{ avgScore }}</span>
				</div>
			</template>

			<el-table :data="courseAverages" style="width: 100%">
				<el-table-column type="index" label="序号" width="80" />
				<el-table-column prop="courseId" label="课程编号" width="150" />
				<el-table-column prop="courseName" label="课程名称" width="200" />
				<el-table-column prop="courseCredit" label="学分" width="100" />
				<el-table-column prop="score" label="成绩" width="120" />
			</el-table>
		</el-card>
	</div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { ElMessage } from 'element-plus'

//后续可以替换 API 调用
const singleScores = ref([
	{ stuId: '2023001', courseId: 'C101', score: 85, courseCredit: 3, isPass: true },
	{ stuId: '2023001', courseId: 'C102', score: 58, courseCredit: 2, isPass: false },
	{ stuId: '2023001', courseId: 'C103', score: 72, courseCredit: 3, isPass: true }
])

const courseAverages = ref([
	{ courseId: 'C101', courseName: '高等数学', courseCredit: 3, score: 82 },
	{ courseId: 'C102', courseName: '大学英语', courseCredit: 2, score: 65 },
	{ courseId: 'C103', courseName: '数据结构', courseCredit: 3, score: 74 }
])

// 通过数
const passCount = computed(() => {
	return singleScores.value.filter(item => item.isPass).length
})

// 平均成绩
const avgScore = computed(() => {
	if (courseAverages.value.length === 0) return 0
	const sum = courseAverages.value.reduce((acc, cur) => acc + cur.score, 0)
	return (sum / courseAverages.value.length).toFixed(2)
})

// 生命周期，后续这里可加请求
onMounted(() => {
	// TODO: 调接口获取数据
	console.log("学生成绩数据加载完成")
})
</script>

<style scoped>
.student-grades {
	width: 100%;
	display: flex;
		flex-direction: column;
		gap: 10px;
		/* 卡片之间留点间隔 */
		width: 100%;
		margin: 0;
		padding: 0;
}

.grade-card {
	/* width: 100%; */
		/* min-height: 80vh; */
		background: #ffffffcc;
		border-radius: 16px;
		padding: 30px;
		box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
}

.card-header {
	display: flex;
	justify-content: space-between;
	font-weight: bold;
}

.summary {
	color: #409EFF;
}

:deep(.el-table__header .el-scrollbar__view),
:deep(.el-table__body-wrapper .el-scrollbar__view) {
	display: block !important;
	/* 让容器变成块级，避免 inline vertical-align 导致偏移 */
	width: 100% !important;
	/* 确保宽度占满，同步 header/body 宽度 */
	vertical-align: top !important;
	/* 保底：避免任何遗留的 vertical-align 影响 */
	box-sizing: border-box !important;
}
</style>
