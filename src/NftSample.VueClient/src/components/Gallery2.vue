<template>
    <div>
      <Carousel>
        <Slide v-for="nft in nfts" :key="nft.id">
          <div class="nft-item">
            <h3>{{ nft.name }}</h3>
            <img :src="nft.ipfsImage" :alt="nft.name" />
          </div>
        </Slide>
      </Carousel>
    </div>
  </template>
  
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
  
  export default defineComponent({
    name: 'NftCarousel',
    components: {
      Carousel,
      Slide
    },
    setup() {
      const nfts = ref<Nft[]>([]);
  
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
  
      return {
        nfts,
      };
    },
  });
  </script>
  
  <style scoped>
  .nft-item {
    text-align: center;
    margin-bottom: 20px;
  }
  img {
    max-width: 100%;
    height: 100px;
    display: block;
    margin: 0 auto;
  }
  </style>
  