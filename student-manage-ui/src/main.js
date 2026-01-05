import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import { createPinia } from 'pinia'
// 导入路由配置（确保路径正确，通常是 src/router/index.js）
import router from './router'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
// 3. 创建 Pinia 实例
const pinia = createPinia()
createApp(App)
.use(pinia)    // 先安装 Pinia
.use(router)
.use(ElementPlus)
.mount('#app')
