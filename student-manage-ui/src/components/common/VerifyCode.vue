<!-- src/components/common/VerifyCode.vue -->
<template>
  <canvas
    ref="canvasRef"
    @click="handleRefresh"
    :width="width"
    :height="height"
    class="verify-code-canvas"
  ></canvas>
</template>

<script setup>
import { ref, onMounted, defineProps} from 'vue'

// 组件属性
const props = defineProps({
  // 绑定的验证码值（v-model）
  modelValue: {
    type: String,
    default: ''
  },
  // 画布宽度
  width: {
    type: Number,
    default: 120
  },
  // 画布高度
  height: {
    type: Number,
    default: 40
  },
  // 验证码长度
  length: {
    type: Number,
    default: 4
  }
})

// 组件事件
const emit = defineEmits(['update:modelValue', 'refresh'])

// 画布引用
const canvasRef = ref(null)
// 验证码字符集（排除易混淆字符）
const chars = '0123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz'

// 生成随机验证码
const generateCode = () => {
  let code = ''
  for (let i = 0; i < props.length; i++) {
    const randomIndex = Math.floor(Math.random() * chars.length)
    code += chars[randomIndex]
  }
  // 更新v-model绑定值
  emit('update:modelValue', code)
  return code
}

// 绘制验证码
const drawCode = () => {
  const canvas = canvasRef.value
  if (!canvas) return

  const ctx = canvas.getContext('2d')
  // 清空画布
  ctx.clearRect(0, 0, props.width, props.height)

  // 绘制背景
  ctx.fillStyle = '#f3f4f6'
  ctx.fillRect(0, 0, props.width, props.height)

  // 生成验证码
  const code = generateCode()

  // 绘制文本
  ctx.font = 'bold 20px Arial'
  ctx.textAlign = 'center'
  ctx.textBaseline = 'middle'

  // 每个字符随机位置和颜色
  for (let i = 0; i < code.length; i++) {
    // 随机颜色
    ctx.fillStyle = `rgb(${Math.random() * 100}, ${Math.random() * 100}, ${Math.random() * 150})`
    // 随机旋转角度
    const rotate = (Math.random() - 0.5) * 0.5
    // 字符位置
    const x = props.width / (code.length + 1) * (i + 1)
    const y = props.height / 2

    ctx.save()
    ctx.translate(x, y)
    ctx.rotate(rotate)
    ctx.fillText(code[i], 0, 0)
    ctx.restore()
  }

  // 绘制干扰线
  for (let i = 0; i < 3; i++) {
    ctx.strokeStyle = `rgb(${Math.random() * 150}, ${Math.random() * 150}, ${Math.random() * 150})`
    ctx.beginPath()
    ctx.moveTo(Math.random() * props.width, Math.random() * props.height)
    ctx.lineTo(Math.random() * props.width, Math.random() * props.height)
    ctx.stroke()
  }

  // 绘制干扰点
  for (let i = 0; i < 30; i++) {
    ctx.fillStyle = `rgb(${Math.random() * 200}, ${Math.random() * 200}, ${Math.random() * 200})`
    ctx.beginPath()
    ctx.arc(
      Math.random() * props.width,
      Math.random() * props.height,
      1,
      0,
      2 * Math.PI
    )
    ctx.fill()
  }
}

// 点击刷新验证码
const handleRefresh = () => {
  drawCode()
  emit('refresh')
}

// 组件挂载时绘制验证码
onMounted(() => {
  drawCode()
})
</script>

<style scoped>
.verify-code-canvas {
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
}
</style>