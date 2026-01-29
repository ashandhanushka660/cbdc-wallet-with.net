<template>
  <q-page class="q-pa-lg bg-dark text-white">
    <div class="row q-col-gutter-lg">
      <div class="col-12">
        <div class="row items-center justify-between q-mb-xl">
           <div>
             <div class="text-h4 text-weight-bolder">Ecosystem Services</div>
             <div class="text-subtitle2 text-grey-5">Integrated sovereign utilities and financial hubs</div>
           </div>
           <q-input dark dense filled v-model="search" placeholder="Search services..." style="width: 300px">
              <template v-slot:append> <q-icon name="search" /> </template>
           </q-input>
        </div>

        <!-- Categories -->
        <div class="row q-col-gutter-lg">
          <div v-for="service in filteredServices" :key="service.title" class="col-12 col-sm-6 col-md-4 col-lg-3">
            <q-card flat class="service-card glass-card q-pa-md cursor-pointer" @click="handleService(service)">
              <q-card-section>
                <div class="row items-center justify-between q-mb-lg">
                  <div class="icon-box" :style="{ background: service.color + '20' }">
                    <q-icon :name="service.icon" :color="service.color" size="32px" />
                  </div>
                  <q-badge v-if="service.verified" color="primary" rounded label="Official" />
                </div>
                <div class="text-h6 text-weight-bold q-mb-xs">{{ service.title }}</div>
                <p class="text-caption text-grey-5 leading-relaxed">{{ service.description }}</p>
              </q-card-section>
              
              <q-card-actions align="right">
                <q-btn flat dense icon="arrow_forward" color="primary" />
              </q-card-actions>
            </q-card>
          </div>
        </div>
      </div>

      <!-- Promo Section -->
      <div class="col-12 q-mt-xl">
        <q-card flat class="promo-card q-pa-xl text-white overflow-hidden relative-position">
          <div class="absolute-full bg-noise opacity-10"></div>
          <div class="row items-center q-col-gutter-xl relative-position z-10">
            <div class="col-12 col-md-8">
              <div class="text-h3 text-weight-bolder q-mb-md">Sovereign Bonds Hub</div>
              <p class="text-h6 opacity-80 max-w-600">
                Directly purchase and manage government-backed digital bonds with instant CBDC settlement and automated coupon payments.
              </p>
              <q-btn unelevated color="white" text-color="dark" label="Explore Investments" class="q-px-xl q-mt-md" no-caps rounded />
            </div>
            <div class="col-12 col-md-4 text-center">
              <q-icon name="trending_up" size="140px" color="white" class="opacity-30" />
            </div>
          </div>
        </q-card>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const search = ref('')

const services = [
  { 
    title: 'Utility Settlement', 
    icon: 'electric_bolt', 
    color: 'warning', 
    description: 'Pay electricity, water, and gas bills instantly using your CBDC reserves.', 
    verified: true 
  },
  { 
    title: 'Tax Compliance', 
    icon: 'gavel', 
    color: 'blue-4', 
    description: 'Direct integration with national revenue authorities for automated VAT/Income tax payments.', 
    verified: true 
  },
  { 
    title: 'Pension Hub', 
    icon: 'savings', 
    color: 'green-4', 
    description: 'Monitor government pension fund contributions and manage social security payouts.', 
    verified: true 
  },
  { 
    title: 'Digital Bonds', 
    icon: 'trending_up', 
    color: 'indigo-4', 
    description: 'Invest in sovereign digital bonds with high-yield returns and atomic finality.', 
    verified: true 
  },
  { 
    title: 'Transport Pass', 
    icon: 'directions_bus', 
    color: 'deep-orange-4', 
    description: 'Reload national transit cards and pay for tolls via NFC/QR atomic protocols.', 
    verified: false 
  },
  { 
    title: 'Health Registry', 
    icon: 'health_and_safety', 
    color: 'red-4', 
    description: 'Access secure medical records and pay for institutional healthcare services.', 
    verified: false 
  },
  { 
    title: 'Identity Portal', 
    icon: 'assignment_ind', 
    color: 'cyan-4', 
    description: 'Manage NID updates and government document authentication services.', 
    verified: true 
  },
  { 
    title: 'Trade Finance', 
    icon: 'account_balance', 
    color: 'purple-4', 
    description: 'Enterprise-grade letters of credit and decentralized supply chain financing.', 
    verified: true 
  }
]

const filteredServices = computed(() => {
  if (!search.value) return services
  const query = search.value.toLowerCase()
  return services.filter(s => 
    s.title.toLowerCase().includes(query) || 
    s.description.toLowerCase().includes(query)
  )
})

function handleService(service) {
  $q.notify({
    message: `Redirecting to ${service.title} portal...`,
    color: 'primary',
    position: 'top',
    icon: service.icon
  })
}
</script>

<style lang="scss" scoped>
.glass-card {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 20px;
}
.service-card {
  transition: all 0.3s ease;
  min-height: 240px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  &:hover {
    transform: translateY(-10px);
    background: rgba(255, 255, 255, 0.08);
    border-color: rgba(0, 210, 255, 0.4);
    box-shadow: 0 10px 30px rgba(0, 210, 255, 0.1);
  }
}
.icon-box {
  width: 64px;
  height: 64px;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.max-w-600 { max-width: 600px; }
.leading-relaxed { line-height: 1.6; }

.promo-card {
  background: linear-gradient(135deg, #3a7bd5 0%, #00d2ff 100%);
  border-radius: 30px;
  box-shadow: 0 20px 50px rgba(0, 210, 255, 0.3);
}

.bg-noise {
  background-image: url("https://www.transparenttextures.com/patterns/carbon-fibre.png");
}

.z-10 { z-index: 10; }
</style>
