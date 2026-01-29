<template>
  <q-page class="bg-dark text-white q-pb-xl">
    <div class="container q-mx-auto q-px-md q-pt-xl">
      <!-- Header -->
      <div class="text-center q-mb-xl fade-in">
        <h1 class="text-h2 md:text-h1 text-weight-bolder text-primary-gradient q-mb-md">Research Team</h1>
        <p class="text-h6 text-grey-5 max-w-700 q-mx-auto">
          The architects behind the sovereign digital economy and distributed ledgers.
        </p>
        <div class="divider q-mx-auto q-mt-md"></div>
      </div>

      <!-- Team Grid -->
      <div class="row q-col-gutter-xl justify-center">
        <div v-for="member in team" :key="member.name" class="col-12 col-sm-6 col-md-4">
          <q-card flat class="team-card glass-card overflow-hidden">
            <div class="image-container">
              <img :src="member.image" :alt="member.name" class="team-image" />
              <div class="image-overlay"></div>
            </div>
            
            <q-card-section class="text-center q-pa-lg">
              <div class="text-h5 text-weight-bolder q-mb-xs">{{ member.name }}</div>
              <div class="text-subtitle2 text-primary text-weight-bold uppercase-title q-mb-md">{{ member.role }}</div>
              <p class="text-grey-5 q-mb-lg">{{ member.bio }}</p>
              
              <div class="row justify-center q-gutter-md">
                <q-btn flat round color="grey-5" icon="link" size="sm" class="hover-primary" />
                <q-btn flat round color="grey-5" icon="share" size="sm" class="hover-primary" />
              </div>
            </q-card-section>
          </q-card>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
const team = [
  {
    name: 'P.A Dhanushka',
    role: 'RESEARCHER (CBDC)',
    bio: 'Leading critical research on Central Bank Digital Currencies (CBDCs) and digital currency pilots, focusing on national economic integration.',
    image: '/team/dhanushka.png'
  },
  {
    name: 'D.M.N.D Dissanayaka',
    role: 'RESEARCHER (BLOCKCHAIN LAYERS)',
    bio: 'Exploring blockchain utilities beyond cryptocurrency, including supply chain integrity, secure health records, and land registries.',
    image: '/team/dissanayaka.png'
  },
  {
    name: 'A.S.C.K Nissanka',
    role: 'RESEARCHER (FINTECH)',
    bio: 'Specializes in the evolution of FinTech and the Digital Economy, researching institutional transformation and sovereign financial tech.',
    image: '/team/nissanka.png'
  }
]
</script>

<style lang="scss" scoped>
.container {
  max-width: 1200px;
}
.max-w-700 {
  max-width: 700px;
}
.divider {
  width: 100px;
  height: 4px;
  background: #00d2ff;
  border-radius: 2px;
}
.team-card {
  height: 100%;
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 24px;
  transition: all 0.3s ease;
  &:hover {
    transform: translateY(-10px);
    border-color: rgba(0, 210, 255, 0.4);
    box-shadow: 0 10px 30px rgba(0, 210, 255, 0.1);
    .team-image {
      transform: scale(1.05);
    }
  }
}
.image-container {
  height: 350px;
  overflow: hidden;
  position: relative;
}
.team-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s ease;
}
.image-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(to bottom, transparent 60%, rgba(10, 14, 39, 0.8));
}
.uppercase-title {
  letter-spacing: 1px;
  font-size: 0.8rem;
}
.hover-primary:hover {
  color: #00d2ff !important;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
.fade-in {
  animation: fadeIn 0.8s ease-out;
}
</style>
