<template>
	<div class="log-list">
		<!-- 外层卡片容器 -->
		<div class="content-card">
			<!-- 区域1：操作按钮区 -->
			<div class="action-bar">
				<el-button type="primary" @click="fetchLogs">刷新</el-button>
				<el-button type="success">导出</el-button>
			</div>

			<!-- 区域2：查询条件区 -->
			<div class="filter-bar">
				<el-form :inline="true" label-width="100px">
					<el-form-item label="用户ID：">
						<el-input v-model="filters.userId" placeholder="请输入用户ID"></el-input>
					</el-form-item>
					<el-form-item label="用户类别：">
						<el-input v-model="filters.userType" placeholder="请输入用户类别"></el-input>
					</el-form-item>
					<el-form-item label="操作类型：">
						<el-input v-model="filters.actionType" placeholder="请输入操作类型"></el-input>
					</el-form-item>
					<el-form-item label="登录时间：">
						<el-date-picker v-model="filters.dateRange" type="daterange" start-placeholder="开始日期"
							end-placeholder="结束日期" value-format="YYYY-MM-DD" />
					</el-form-item>
					<el-form-item>
						<el-button type="info" @click="searchLogs">查询</el-button>
					</el-form-item>
				</el-form>
			</div>

			<!-- 区域3：数据展示区 -->
			<div class="table-area">
				<el-table :data="logData" style="width: 100%">
					<!-- <el-table-column prop="logId" label="日志ID" width="100" /> -->
					<el-table-column prop="userId" label="用户ID" width="120" />
					<el-table-column prop="userType" label="用户类型" width="120" />
					<el-table-column prop="operationType" label="操作类型" width="150" />
					<el-table-column prop="loginTime" label="登录时间" width="200" :formatter="formatDate" />
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
		</div>
	</div>
</template>

<script setup>
import { ref, onMounted } from "vue"
import { ElMessage } from "element-plus"
import { getAllLogs, queryLogs } from "@/api/loginLogApi.js"

// 查询条件
const filters = ref({
	userId: '',
	userType: '',
	actionType: '',
	dateRange: [],
})

// 表格数据
const logData = ref([])

// 分页数据
const pagination = ref({
	currentPage: 1,
	pageSize: 10,
	total: 0
})

// 全部日志数据（用于前端分页）
const allLogs = ref([])

// 初始化加载
const loadLogs = async () => {
	try {
		const res = await getAllLogs()
		console.log("res:", res)
		console.log("res.data:", res.data)
		allLogs.value = res.data || []
		pagination.value.total = allLogs.value.length
		// 更新当前页数据
		updatePageData()
	} catch (e) {
		ElMessage.error("加载日志失败")
	}
}

// 更新当前页显示的数据
const updatePageData = () => {
	const start = (pagination.value.currentPage - 1) * pagination.value.pageSize
	const end = start + pagination.value.pageSize
	logData.value = allLogs.value.slice(start, end)
}

// 页码改变
const handlePageChange = (page) => {
	pagination.value.currentPage = page
	updatePageData()
}

// 每页条数改变
const handleSizeChange = (size) => {
	pagination.value.pageSize = size
	pagination.value.currentPage = 1 // 重置到第一页
	updatePageData()
}

// 格式化日期
const formatDate = (row, column, cellValue) => {
	if (!cellValue) return ""
	return new Date(cellValue).toLocaleString()
}

// 刷新日志
const fetchLogs = () => {
	console.log('刷新日志...')
	loadLogs()
}

// 按条件查询日志
const searchLogs = async () => {
	console.log('按条件查询日志', filters.value)
	try {
		const res = await queryLogs(filters.value)
		allLogs.value = res.data || []
		pagination.value.total = allLogs.value.length
		pagination.value.currentPage = 1 // 重置到第一页
		updatePageData()
	} catch (e) {
		ElMessage.error("查询日志失败")
	}
}

onMounted(() => {
	loadLogs()
})
</script>

<style scoped>
.log-list {
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
