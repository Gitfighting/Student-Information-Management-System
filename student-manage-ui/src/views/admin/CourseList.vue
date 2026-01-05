<template>
	<div class="course-list">
		<!-- 外层卡片容器 -->
		<div class="content-card">
			<!-- 区域1：操作按钮区 -->
			<div class="action-bar">
				<el-button type="primary" @click="handleQuery">查询</el-button>
				<el-button @click="resetQuery">重置</el-button>
				<el-button type="success" @click="openAddDialog">新增课程</el-button>
			</div>

			<!-- 区域2：查询条件区 -->
			<div class="filter-bar">
				<el-form :inline="true" :model="queryForm" label-width="100px">
					<el-form-item label="课程编号：">
						<el-input v-model="queryForm.courseId" placeholder="请输入课程编号" />
					</el-form-item>
					<el-form-item label="课程名：">
						<el-input v-model="queryForm.courseName" placeholder="请输入课程名称" />
					</el-form-item>
					<el-form-item label="先修课程：">
						<el-input v-model="queryForm.preCourseId" placeholder="请输入先修课程" />
					</el-form-item>
					<el-form-item label="学分：">
						<el-input v-model="queryForm.courseCredit" placeholder="请输入学分" />
					</el-form-item>
				</el-form>
			</div>

			<!-- 区域3：数据展示区 -->
			<div class="table-area">
				<el-table :data="courseList" style="width: 100%">
					<el-table-column prop="courseId" label="课程编号" width="150" />
					<el-table-column prop="courseName" label="课程名" width="200" />
					<el-table-column prop="preCourseName" label="先修课程" width="200" />
					<el-table-column prop="courseCredit" label="学分"  />
					<el-table-column label="操作" width="220">
						<template #default="scope">
							<el-button size="small" @click="openEditDialog(scope.row)">编辑</el-button>
							<el-button size="small" type="danger" @click="handleDelete(scope.row.courseId)">删除</el-button>
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

			<!-- 区域4：新增/编辑弹窗 -->
			<el-dialog v-model="dialogVisible" :title="dialogTitle" width="500px">
				<el-form :model="formData" label-width="100px">
					<el-form-item label="课程编号">
						<el-input v-model="formData.courseId" :disabled="isEdit" />
					</el-form-item>
					<el-form-item label="课程名">
						<el-input v-model="formData.courseName" />
					</el-form-item>
					<el-form-item label="先修课程">
						<el-select v-model="formData.preCourseId" placeholder="请选择先修课程">
							<el-option v-for="course in courseList" :key="course.courseId" :label="course.courseName"
								:value="course.courseId" />
						</el-select>
					</el-form-item>
					<el-form-item label="学分">
						<el-input v-model="formData.courseCredit" />
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
import { getAllCourses, queryCourses, addCourse, updateCourse, deleteCourse } from '@/api/courseApi.js'

// 数据源（当前页显示）
const courseList = ref([])
const loading = ref(false)

// 全部课程数据（用于前端分页）
const allCourses = ref([])

// 分页数据
const pagination = ref({
	currentPage: 1,
	pageSize: 10,
	total: 0
})

// 查询表单
const queryForm = ref({
	courseId: '',
	courseName: '',
	preCourseId: '',
	courseCredit: ''
})

// 弹窗表单
const dialogVisible = ref(false)
const dialogTitle = ref('')
const isEdit = ref(false)
const formData = ref({
	courseId: '',
	courseName: '',
	preCourseId: '',   // 传给后端的ID
	preCourseName: '',
	courseCredit: ''
})

// 更新当前页显示的数据
const updatePageData = () => {
	const start = (pagination.value.currentPage - 1) * pagination.value.pageSize
	const end = start + pagination.value.pageSize
	courseList.value = allCourses.value.slice(start, end)
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

// 获取全部课程
const fetchCourses = async () => {
	loading.value = true
	const res = await getAllCourses()
	console.log("res:", res)           // axios 原始响应
	console.log("res.data:", res.data) // 后端返回的 JSON
	allCourses.value = res.data || []
	pagination.value.total = allCourses.value.length
	updatePageData()
}

// 查询课程
const handleQuery = async () => {
	const res = await queryCourses(queryForm.value)
	allCourses.value = res.data || []
	pagination.value.total = allCourses.value.length
	pagination.value.currentPage = 1
	updatePageData()
}

// 重置查询
const resetQuery = () => {
	queryForm.value = { courseId: '', courseName: '',preCourseName: '', courseCredit: '' }
	fetchCourses()
}

// 打开新增弹窗
const openAddDialog = () => {
	dialogTitle.value = '新增课程'
	isEdit.value = false
	formData.value = { courseId: '', courseNamee: '', preCourseName: '', courseCredit: '' }
	dialogVisible.value = true
}

// 打开编辑弹窗
const openEditDialog = (row) => {
	dialogTitle.value = '编辑课程'
	isEdit.value = true
	formData.value = { ...row }
	dialogVisible.value = true
}

// 提交新增/编辑
const handleSubmit = async () => {
		let res
		if (isEdit.value) {
			res = await updateCourse(formData.value)
		} else {
			res = await addCourse(formData.value)
		}

			ElMessage.success(isEdit.value ? '修改成功' : '新增成功')
			dialogVisible.value = false
			fetchCourses()

}

// 删除课程
const handleDelete = (courseId) => {
	ElMessageBox.confirm('确定删除该课程吗？', '提示', {
		type: 'warning'
	}).then(async () => {

		const res = await deleteCourse(courseId)
		ElMessage.success('删除成功')
		fetchCourses()

	})
}

onMounted(() => {
	fetchCourses()
})
</script>

<style scoped>
.course-list {
	width: 100%;
	height: 100%;
	margin: 0;
	padding: 0;

	/* 右边内容与导航栏风格一致 */
	display: flex;
	justify-content: center;
	/* align-items: flex-start; */
}

/* 套用 admin-profile 的卡片风格 */
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
