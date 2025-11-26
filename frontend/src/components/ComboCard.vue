<template>
  <div @click="navigateToDetail" class="bg-white rounded-2xl overflow-hidden shadow-md hover:shadow-xl transition-all duration-300 group flex flex-col h-full cursor-pointer">
    <!-- Combo Image -->
    <div class="relative aspect-[4/3] overflow-hidden bg-gray-100">
      <img 
        :src="imageUrl" 
        :alt="combo.name"
        @error="handleImageError"
        class="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
      />
      
      <!-- Combo badge -->
      <div class="absolute top-3 left-3">
        <span class="bg-purple-600 text-white text-xs font-bold px-3 py-1.5 rounded-full shadow-lg">
          Combo
        </span>
      </div>
      
      <!-- Action buttons -->
      <div class="absolute bottom-3 right-3 flex gap-2">
        <button 
          @click.stop="addToCart"
          class="bg-red-500 text-white p-2.5 rounded-full hover:bg-red-600 transition-colors shadow-lg"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
          </svg>
        </button>
        <button 
          @click.stop="navigateToDetail"
          class="bg-gray-800 text-white p-2.5 rounded-full hover:bg-gray-900 transition-colors shadow-lg"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
        </button>
      </div>
    </div>
    
    <!-- Combo Info -->
    <div class="p-3 flex-1 flex flex-col">
      <!-- Title -->
      <h3 class="font-semibold text-gray-900 text-sm mb-1.5 line-clamp-1">
        {{ combo.name }}
      </h3>
      
      <!-- Description -->
      <p class="text-xs text-gray-600 mb-2 line-clamp-2">
        {{ combo.description || 'Combo đặc biệt với giá ưu đãi.' }}
      </p>
      
      <!-- Price -->
      <div class="mt-auto">
        <span class="text-lg font-bold text-red-500">{{ formatPrice(combo.price) }}</span>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

export default {
  name: 'ComboCard',
  props: {
    combo: {
      type: Object,
      required: true
    }
  },
  setup(props, context) {
    const router = useRouter()
    const isAdding = ref(false)
    const imageError = ref(false)

    const imageUrl = computed(() => {
      if (imageError.value) {
        return `https://ui-avatars.com/api/?name=${encodeURIComponent(props.combo.name)}&size=400&background=fef3c7&color=92400e&bold=true`
      }
      
      let url = props.combo.imageUrl;
      if (url && url.startsWith('photo-')) {
          return `https://images.unsplash.com/${url}`;
      }
      
      return url || `https://source.unsplash.com/400x300/?food,combo,meal`
    })

    const handleImageError = () => {
      imageError.value = true
    }

    const navigateToDetail = () => {
      router.push({ name: 'combo-detail', params: { id: props.combo.comboId } })
    }

    const addToCart = async () => {
      isAdding.value = true
      // Emit event to parent
      context.emit('add-to-cart', props.combo)
      await new Promise(resolve => setTimeout(resolve, 600))
      isAdding.value = false
    }

    const formatPrice = (price) => {
      return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price * 1000)
    }

    return {
      isAdding,
      imageError,
      imageUrl,
      handleImageError,
      addToCart,
      formatPrice,
      navigateToDetail
    }
  }
}
</script>
