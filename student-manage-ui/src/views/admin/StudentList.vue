<template>
	<div class="student-list">
		<!-- 外层卡片容器 -->
		<div class="content-card">
			<!-- 区域1：操作按钮区 -->
			<div class="action-bar">
				<el-button type="primary" @click="openAdd">添加</el-button>
				<!-- <el-button type="success">修改</el-button> -->
				<!-- <el-button type="danger">删除</el-button> -->
				<el-button type="info" @click="handleQuery">查询</el-button>
			</div>

			<!-- 区域2：查询条件区 -->
			<el-form :inline="true" label-width="80px">
				<el-form-item label="学号：">
					<el-input placeholder="请输入学号" v-model="queryForm.stuId"></el-input>
				</el-form-item>
				<el-form-item label="姓名：">
					<el-input placeholder="请输入姓名" v-model="queryForm.stuName"></el-input>
				</el-form-item>
				<el-form-item label="性别：">
					<el-input placeholder="请输入性别" v-model="queryForm.stuGender"></el-input>
				</el-form-item>
				<!-- 年龄输入框（注意：后续需转换为数字类型） -->
				<el-form-item label="年龄：">
					<el-input placeholder="请输入年龄" v-model="queryForm.stuAge"></el-input>
				</el-form-item>
				<el-form-item label="专业：">
					<el-input placeholder="请输入专业" v-model="queryForm.stuMajor"></el-input>
				</el-form-item>
			</el-form>

			<!-- 区域3：数据展示区 -->
			<div class="table-area">
				<el-table :data="studentList" style="width: 100%">
					<el-table-column prop="stuId" label="学号" width="120" />
					<el-table-column prop="stuName" label="姓名" width="150" />
					<el-table-column prop="stuGender" label="性别" width="100" />
					<el-table-column prop="stuAge" label="年龄" width="100" />
					<el-table-column prop="stuMajor" label="专业" />
					<el-table-column label="操作" width="200">
						<template #default="scope">
							<el-button size="small" @click="openEdit(scope.row)">修改</el-button>
							<el-button size="small" type="danger" @click="handleDelete(scope.row.stuId)">删除</el-button>
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

			<!-- 添加学生弹窗 -->
			<el-dialog title="添加学生" v-model="addDialogVisible" width="600px">
				<el-form :model="addForm" label-width="100px">
					<el-form-item label="学号">
						<el-input v-model="addForm.stuId" />
					</el-form-item>
					<el-form-item label="姓名">
						<el-input v-model="addForm.stuName" />
					</el-form-item>
					<el-form-item label="性别">
						<el-select v-model="addForm.stuGender" placeholder="请选择">
							<el-option label="男" value="男" />
							<el-option label="女" value="女" />
						</el-select>
					</el-form-item>
					<el-form-item label="年龄">
						<el-input v-model="addForm.stuAge" type="number" />
					</el-form-item>
					<el-form-item label="专业">
						<el-input v-model="addForm.stuMajor" />
					</el-form-item>
					<el-form-item label="出生日期">
						<el-date-picker v-model="addForm.stuBirth" type="date" placeholder="选择日期" />
					</el-form-item>
					<el-form-item label="邮箱">
						<el-input v-model="addForm.stuEmail" />
					</el-form-item>
					<el-form-item label="电话">
						<el-input v-model="addForm.stuPhone" />
					</el-form-item>
					<el-form-item label="密码">
						<el-input v-model="addForm.stuPwd" type="password" />
					</el-form-item>
				</el-form>
				<template #footer>
					<el-button @click="addDialogVisible = false">取消</el-button>
					<el-button type="primary" @click="submitAdd">确认</el-button>
				</template>
			</el-dialog>

			<!-- 修改学生弹窗 -->
			<el-dialog title="修改学生" v-model="editDialogVisible" width="600px">
				<el-form :model="editForm" label-width="100px">
					<el-form-item label="学号">
						<el-input v-model="editForm.stuId" disabled />
					</el-form-item>
					<el-form-item label="姓名">
						<el-input v-model="editForm.stuName" />
					</el-form-item>
					<el-form-item label="性别">
						<el-select v-model="editForm.stuGender" placeholder="请选择">
							<el-option label="男" value="男" />
							<el-option label="女" value="女" />
						</el-select>
					</el-form-item>
					<el-form-item label="年龄">
						<el-input v-model="editForm.stuAge" type="number" />
					</el-form-item>
					<el-form-item label="专业">
						<el-input v-model="editForm.stuMajor" />
					</el-form-item>
					<el-form-item label="出生日期">
						<el-date-picker v-model="editForm.stuBirth" type="date" placeholder="选择日期" />
					</el-form-item>
					<el-form-item label="邮箱">
						<el-input v-model="editForm.stuEmail" />
					</el-form-item>
					<el-form-item label="电话">
						<el-input v-model="editForm.stuPhone" />
					</el-form-item>
				</el-form>
				<template #footer>
					<el-button @click="editDialogVisible = false">取消</el-button>
					<el-button type="primary" @click="submitEdit">保存</el-button>
				</template>
			</el-dialog>


		</div>
	</div>
</template>
<script setup>
import { ref, onMounted } from "vue"
import { ElMessage, ElMessageBox } from "element-plus"
import { getAllStudents, queryStudents, addStudent, updateStudent, deleteStudent } from "@/api/studentApi.js"

// 学生数据表（当前页显示）
const studentList = ref([])

// 全部学生数据（用于前端分页）
const allStudents = ref([])

// 分页数据
const pagination = ref({
	currentPage: 1,
	pageSize: 10,
	total: 0
})

// 查询条件
const queryForm = ref({
	stuId: "",
	stuName: "",
	stuGender: "",
	stuAge: "",
	stuMajor: ""
})

// 弹窗控制
const addDialogVisible = ref(false)
const editDialogVisible = ref(false)

// 添加表单
const addForm = ref({
	stuId: "",
	stuName: "",
	stuGender: "",
	stuAge: "",
	stuMajor: "",
	stuBirth: "",
	stuEmail: "",
	stuPhone: "",
	stuPwd: ""
})

// 修改表单
const editForm = ref({})

// 更新当前页显示的数据
const updatePageData = () => {
	const start = (pagination.value.currentPage - 1) * pagination.value.pageSize
	const end = start + pagination.value.pageSize
	studentList.value = allStudents.value.slice(start, end)
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

// 1. 获取所有学生
const loadStudents = async () => {
	const res = await getAllStudents()  // 调用接口
	console.log("res:", res)           // axios 原始响应
	console.log("res.data:", res.data) // 后端返回的 JSON
	allStudents.value = res.data || []
	pagination.value.total = allStudents.value.length
	updatePageData()
}

// 2. 多条件查询
const handleQuery = async () => {
	const res = await queryStudents(queryForm.value)
	allStudents.value = res.data || []
	pagination.value.total = allStudents.value.length
	pagination.value.currentPage = 1
	updatePageData()
}

// 3. 打开添加弹窗
const openAdd = () => {
	addForm.value = {}
	addDialogVisible.value = true
}

// 4. 提交添加
const submitAdd = async () => {
	await addStudent(addForm.value)
	ElMessage.success("添加成功")
	addDialogVisible.value = false
	loadStudents()
}

// 5. 打开修改弹窗
const openEdit = (row) => {
	editForm.value = { ...row }
	editDialogVisible.value = true
}

// 6. 提交修改
const submitEdit = async () => {
	await updateStudent(editForm.value)
	ElMessage.success("修改成功")
	editDialogVisible.value = false
	loadStudents()
}

// 7. 删除学生
const handleDelete = (stuId) => {
	ElMessageBox.confirm('确定删除该课程吗？', '提示', {
		type: 'warning'
	}).then(async () => {

		const res = await deleteStudent(stuId)
		ElMessage.success('删除成功')
		loadStudents()

	})
}

// 页面初始化时加载数据
onMounted(() => {
	loadStudents()
})
</script>

<style scoped>
.student-list {
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

.action-bar {
	display: flex;
	gap: 16px;
	margin-bottom: 40px;
}

.filter-bar {
	margin-bottom: 20px;
}

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
