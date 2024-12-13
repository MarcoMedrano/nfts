<script lang="ts">
  import 'vue3-carousel/dist/carousel.css'
  import { defineComponent, ref, onMounted } from 'vue';
  import { Carousel, Slide } from 'vue3-carousel';
  
  interface Nft {
    id: number;
    userId: string;
    name: string;
    ipfsImage: string;
  }

  const currentSlide = ref(0);

const slideTo = (nextSlide: any) => (currentSlide.value = nextSlide);
  
  export default defineComponent({
    name: 'NftCarousel',
    components: {
      Carousel,
      Slide
    },
    setup() {
      const nfts = ref<Nft[]>([]);
      const currentSlide = ref(0);

      const fetchNfts = async () => {
        try {
          const response = await fetch('http://localhost:8080/api/nfts');
          nfts.value = await response.json();
        } catch (error) {
          console.error('Failed to fetch NFTs:', error);
        }
      };
  
      onMounted(() => {
        fetchNfts();
      });

      const slideTo = (index: number) => { currentSlide.value = index};
  
      return {
        nfts,
        currentSlide,
        slideTo,
      };
    },
  });
  </script>
  
  <template>
    <div>
      <Carousel :items-to-show=1 :wrap-around=true :transition=500 v-model="currentSlide">
        <Slide v-for="nft in nfts" :key="nft.id">
          <div class="nft-item">
              <img :src="nft.ipfsImage" :alt="nft.name" />
              <h2>{{ nft.name }}</h2>
          </div>
        </Slide>
      </Carousel>  
      <Carousel :items-to-show=3.5 :gap=10 :wrap-around=true :transition=500 v-model="currentSlide">
        <Slide v-for="nft in nfts" :key="nft.id">
          <div class="nft-item" @click="slideTo(nft.id - 1)">
            <img :src="nft.ipfsImage" :alt="nft.name" />
          </div>
        </Slide>
      </Carousel>
    </div>
  </template>

  <style scoped>
  .nft-item {
    text-align: center;
    margin-bottom: 20px;
  }
  img {
    max-width: 100%;
    height:auto;
    display: block;
    margin: 0 auto;
  }
  </style>
  