<template>
  <q-page class="q-pa-lg bg-dark text-white">
    <div class="max-w-900 q-mx-auto">
      <div class="row items-center q-mb-xl">
        <q-btn flat round icon="arrow_back" to="/dashboard" color="grey-5" class="q-mr-md" />
        <h1 class="text-h4 text-weight-bolder">Institutional Settings</h1>
      </div>

      <div class="row q-col-gutter-lg">
        <!-- Profile Settings -->
        <div class="col-12 col-md-8">
          <q-card flat class="glass-card q-pa-lg q-mb-lg">
            <div class="text-h6 text-weight-bold q-mb-lg row items-center">
              <q-icon name="person" color="primary" class="q-mr-sm" size="24px" />
              Sovereign Profile
            </div>
            
            <q-form class="q-gutter-md">
              <div class="row q-col-gutter-md">
                <div class="col-12 col-sm-6">
                  <q-input dark filled v-model="profile.firstName" label="First Name" />
                </div>
                <div class="col-12 col-sm-6">
                  <q-input dark filled v-model="profile.lastName" label="Last Name" />
                </div>
              </div>
              <q-input dark filled v-model="profile.email" label="Institutional Email" readonly>
                 <template v-slot:append>
                    <q-badge color="green-4" rounded label="Verified" size="sm" />
                 </template>
              </q-input>
              <q-btn unelevated color="primary" label="Save Profile Changes" class="q-px-lg" no-caps rounded @click="saveSettings" />
            </q-form>
          </q-card>

          <q-card flat class="glass-card q-pa-lg">
            <div class="text-h6 text-weight-bold q-mb-lg row items-center">
              <q-icon name="security" color="accent" class="q-mr-sm" size="24px" />
              Infrastructure Security
            </div>
            
            <q-list dark separator padding>
              <q-item tag="label" v-ripple>
                <q-item-section>
                  <q-item-label>Multi-Factor Authentication (MFA)</q-item-label>
                  <q-item-label caption class="text-grey-5">Require an extra layer of security for transfers.</q-item-label>
                </q-item-section>
                <q-item-section side>
                  <q-toggle v-model="security.mfa" color="accent" />
                </q-item-section>
              </q-item>
              
              <q-item tag="label" v-ripple>
                <q-item-section>
                  <q-item-label>Biometric Confirmation</q-item-label>
                  <q-item-label caption class="text-grey-5">Use FaceID or Fingerprint to authorize atomic settlements.</q-item-label>
                </q-item-section>
                <q-item-section side>
                  <q-toggle v-model="security.biometrics" color="accent" />
                </q-item-section>
              </q-item>
            </q-list>
          </q-card>
        </div>

        <!-- System Stats / Activity -->
        <div class="col-12 col-md-4">
          <q-card flat class="bg-surface q-pa-lg rounded-borders height-100">
            <div class="text-subtitle2 text-grey-5 uppercase tracking-widest q-mb-md">Session Status</div>
            <div class="bg-grey-10 q-pa-md rounded-borders q-mb-md">
               <div class="row justify-between q-mb-xs">
                  <span class="text-grey-4">Environment:</span>
                  <span class="text-green-4 text-weight-bold">INSTITUTIONAL</span>
               </div>
               <div class="row justify-between">
                  <span class="text-grey-4">Uptime:</span>
                  <span class="text-white">99.999%</span>
               </div>
            </div>

            <div class="text-subtitle2 text-grey-5 uppercase tracking-widest q-mb-md q-mt-lg">Login History</div>
            <q-list dense dark>
              <q-item class="q-px-none">
                <q-item-section>
                  <q-item-label class="text-caption">Today, 01:34 AM</q-item-label>
                  <q-item-label caption class="text-grey-6">Windows 11 • New York, US</q-item-label>
                </q-item-section>
              </q-item>
            </q-list>
          </q-card>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const profile = ref({ firstName: '', lastName: '', email: '' })
const security = ref({ mfa: true, biometrics: false })

onMounted(() => {
  const userData = localStorage.getItem('cbdc_user')
  if (userData) {
    const user = JSON.parse(userData)
    profile.value = { 
        firstName: user.firstName, 
        lastName: user.lastName, 
        email: user.email 
    }
  }
})

function saveSettings() {
    $q.notify({
        color: 'green-4',
        icon: 'check_circle',
        message: 'Institutional settings synchronized successfully.'
    })
}
</script>

<style scoped>
.max-w-900 { max-width: 900px; }
.glass-card {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 20px;
}
.bg-surface { background: #111; }
.rounded-borders { border-radius: 20px; }
.uppercase { text-transform: uppercase; }
.tracking-widest { letter-spacing: 2px; }
.height-100 { height: 100%; }
</style>
