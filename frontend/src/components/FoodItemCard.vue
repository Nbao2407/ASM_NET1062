<template>
  <div @click="navigateToDetail" class="bg-white rounded-2xl overflow-hidden shadow-md hover:shadow-xl transition-all duration-300 group flex flex-col h-full cursor-pointer">
    <!-- Product Image -->
    <div class="relative aspect-[4/3] overflow-hidden bg-gray-100">
      <img 
        :src="imageUrl" 
        :alt="item.name"
        @error="handleImageError"
        class="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
      />
      
      <!-- Trending badge -->
      <div v-if="item.isPopular || item.isTrending" class="absolute top-3 left-3">
        <span class="bg-red-500 text-white text-xs font-bold px-3 py-1.5 rounded-full shadow-lg flex items-center gap-1">
          🔥 Trending
        </span>
      </div>
    </div>
    
    <!-- Product Info -->
    <div class="p-4 flex-1 flex flex-col">
      <!-- Category tag & Rating -->
      <div class="flex items-center justify-between mb-2">
        <span class="text-xs font-medium text-red-500">{{ item.category || 'Burger & Cơm' }}</span>
        <div class="flex items-center gap-1">
          <div class="flex">
            <svg v-for="star in 5" :key="star" 
                 :class="star <= Math.floor(item.rating || 4.8) ? 'text-yellow-400 fill-current' : 'text-gray-300'"
                 class="h-3.5 w-3.5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
              <path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/>
            </svg>
          </div>
          <span class="text-sm font-medium text-gray-700 ml-1">{{ item.rating || 4.8 }}</span>
        </div>
      </div>
      
      <!-- Title -->
      <h3 class="font-bold text-gray-900 text-base mb-2 line-clamp-2 min-h-[3rem]">
        {{ item.name }}
      </h3>
      
      <!-- Description -->
      <p class="text-sm text-gray-600 mb-3 line-clamp-1">
        {{ item.description || 'Món ăn ngon, chất lượng cao.' }}
      </p>
      
      <!-- Price & Add button -->
      <div class="flex items-center justify-between mt-auto pt-3 border-t border-gray-100">
        <div class="flex flex-col">
          <span v-if="item.originalPrice" class="text-xs text-gray-400 line-through">{{ formatPrice(item.originalPrice) }}</span>
          <span class="text-xl font-bold text-red-500">{{ formatPrice(item.price) }}</span>
        </div>
        <button 
          @click.stop="addToCart"
          :class="[
            'px-4 py-2 rounded-lg font-medium text-sm transition-all duration-200',
            isAdding 
              ? 'bg-green-500 text-white' 
              : 'bg-red-500 text-white hover:bg-red-600 shadow-md hover:shadow-lg'
          ]"
        >
          <span v-if="!isAdding">+ Thêm</span>
          <span v-else>✓ Đã thêm</span>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

export default {
  name: 'FoodItemCard',
  props: {
    item: {
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
        return `https://ui-avatars.com/api/?name=${encodeURIComponent(props.item.name)}&size=400&background=f3f4f6&color=6b7280&bold=true`
      }
      
      let url = props.item.imageUrl;
      if (url && url.startsWith('photo-')) {
          return `https://images.unsplash.com/${url}`;
      }
      
      return url || `https://source.unsplash.com/400x300/?food,${props.item.category || 'burger'}`
    })

    const handleImageError = () => {
      imageError.value = true
    }

    const navigateToDetail = () => {
      router.push({ name: 'food-detail', params: { id: props.item.foodItemId } })
    }

    const addToCart = async () => {
      isAdding.value = true
      // Emit event to parent
      context.emit('add-to-cart', props.item)
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
