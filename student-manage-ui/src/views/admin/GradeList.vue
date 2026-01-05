<template>
	<div class="grade-list">
		<!-- 外层卡片容器 -->
		<div class="content-card">
			<!-- 操作按钮区 -->
			<div class="action-bar">
				<el-button type="primary" @click="handleQuery">查询</el-button>
				<el-button @click="resetQuery">重置</el-button>
				<el-button type="success" @click="openAddDialog">新增成绩</el-button>
			</div>

			<!-- 查询条件区 -->
			<div class="filter-bar">
				<el-form :inline="true" :model="queryForm" label-width="80px">
					<el-form-item label="学号：">
						<el-input v-model="queryForm.stuId" placeholder="请输入学号" />
					</el-form-item>
					<el-form-item label="课程号：">
						<el-input v-model="queryForm.courseId" placeholder="请输入课程号" />
					</el-form-item>
					<el-form-item label="成绩：">
						<el-input v-model="queryForm.score" placeholder="请输入成绩" />
					</el-form-item>
				</el-form>
			</div>

			<!-- 数据展示区 -->
			<div class="table-area">
				<el-table :data="gradeData" style="width: 100%">
					<el-table-column prop="stuId" label="学号" width="120" />
					<el-table-column prop="stuName" label="姓名" width="150" />
					<el-table-column prop="courseId" label="课程号" width="120" />
					<el-table-column prop="courseName" label="课程名称" width="200" />
					<el-table-column prop="score" label="成绩"  />
					<el-table-column label="操作" width="220">
						<template #default="scope">
							<el-button size="small" @click="openEditDialog(scope.row)">编辑</el-button>
							<el-button size="small" type="danger"
								@click="handleDelete(scope.row.scoreId)">删除</el-button>
						</template>
					</el-table-column>
				</el-table>

				<!-- 分页组件 -->
				<div class="pagination-container">
					<el-pagination
						v-model:current-page="pagination.currentPage"
						v-model:page-size="pagination.pageSize"
						:page-sizes="[10, 20, 50, 100]"
						:total="pagination.total"
						layout="total, sizes, prev, pager, next, jumper"
						@size-change="handleSizeChange"
						@current-change="handlePageChange"
					/>
				</div>
			</div>

			<!-- 新增/编辑弹窗 -->
			<el-dialog v-model="dialogVisible" :title="dialogTitle" width="500px">
				<el-form :model="formData" label-width="80px">
					<el-form-item label="学号">
						<el-input v-model="formData.stuId" :disabled="isEdit" />
					</el-form-item>
					<el-form-item label="课程号">
						<el-input v-model="formData.courseId" :disabled="isEdit" />
					</el-form-item>
					<el-form-item label="成绩">
						<el-input v-model="formData.score" />
					</el-form-item>
				</el-form>
				<template #footer>
					<el-button @click="dialogVisible = false">取消</el-button>
					<el-button type="primary" @click="handleSubmit">确定</el-button>
				</template>
			</el-dialog>
		</div>
	</div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getAllScores, queryScores, addScore, updateScore, deleteScore } from '@/api/scoreApi.js'

// 数据源（当前页显示）
const gradeData = ref([])

// 全部成绩数据（用于前端分页）
const allGrades = ref([])

// 分页数据
const pagination = ref({
	currentPage: 1,
	pageSize: 10,
	total: 0
})

// 查询表单
const queryForm = ref({
	stuId: '',
	courseId: '',
	score: ''
})

// 弹窗表单
const dialogVisible = ref(false)
const dialogTitle = ref('')
const isEdit = ref(false)
const formData = ref({
	stuId: '',
	courseId: '',
	score: ''
})

// 更新当前页显示的数据
const updatePageData = () => {
	const start = (pagination.value.currentPage - 1) * pagination.value.pageSize
	const end = start + pagination.value.pageSize
	gradeData.value = allGrades.value.slice(start, end)
}

// 页码改变
const handlePageChange = (page) => {
	pagination.value.currentPage = page
	updatePageData()
}

// 每页条数改变
const handleSizeChange = (size) => {
	pagination.value.pageSize = size
	pagination.value.currentPage = 1
	updatePageData()
}

// 获取全部成绩
const fetchGrades = async () => {
	const res = await getAllScores()
	console.log('res:', res)
	console.log('res.data:', res.data)
	allGrades.value = res.data || []
	pagination.value.total = allGrades.value.length
	updatePageData()
}

// 查询成绩
const handleQuery = async () => {
	const res = await queryScores(queryForm.value)
	allGrades.value = res.data || []
	pagination.value.total = allGrades.value.length
	pagination.value.currentPage = 1
	updatePageData()
}

// 重置查询
const resetQuery = () => {
	queryForm.value = { stuId: '', courseId: '', score: '' }
	fetchGrades()
}

// 打开新增弹窗
const openAddDialog = () => {
	dialogTitle.value = '新增成绩'
	isEdit.value = false
	formData.value = { stuId: '', courseId: '', score: '' }
	dialogVisible.value = true
}

// 打开编辑弹窗
const openEditDialog = (row) => {
	dialogTitle.value = '编辑成绩'
	isEdit.value = true
	formData.value = { ...row }
	dialogVisible.value = true
}

// 提交新增/编辑
const handleSubmit = async () => {
	let res
	if (isEdit.value) {
		res = await updateScore(formData.value)
	} else {
		res = await addScore(formData.value)
	}

	ElMessage.success(isEdit.value ? '修改成功' : '新增成功')
	dialogVisible.value = false
	fetchGrades()
}

// 删除成绩
const handleDelete = (scoreId) => {
	ElMessageBox.confirm('确定删除该成绩吗？', '提示', {
		type: 'warning'
	}).then(async () => {
		await deleteScore(scoreId)
		ElMessage.success('删除成功')
		fetchGrades()
	})
}

onMounted(() => {
	fetchGrades()
})
</script>


<style scoped>
.grade-list {
	width: 100%;
	height: 100%;
	margin: 0;
	padding: 0;

	display: flex;
	justify-content: center;
}

.content-card {
	width: 100%;
	min-height: 80vh;
	background: #ffffffcc;
	border-radius: 16px;
	padding: 30px;
	box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
}

/* 区域1：操作按钮区 */
.action-bar {
	display: flex;
	gap: 16px;
	margin-bottom: 40px;
}

/* 区域2：查询条件区 */
.filter-bar {
	margin-bottom: 20px;
}

/* 区域3：数据展示区 */
.table-area {
	flex: 1;
}

.pagination-container {
	margin-top: 20px;
	display: flex;
	justify-content: flex-end;
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
