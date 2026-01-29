<template>
  <q-page class="flex flex-center" style="min-height: 100vh; background: #0a0e27;">
    <div class="register-container fade-in-up">
      <div class="text-center q-mb-xl">
        <h1 class="text-gradient text-large q-mb-md">CBDC Wallet</h1>
        <p class="text-secondary" style="font-size: 18px;">Create your digital wallet account</p>
      </div>

      <q-card class="wallet-card shadow-xl" style="max-width: 500px; width: 100%;">
        <q-card-section>
          <q-form @submit="onSubmit" class="q-gutter-md">
            <div>
              <label class="text-small q-mb-xs block">First Name</label>
              <q-input
                v-model="form.firstName"
                filled
                dark
                placeholder="Enter your first name"
                :rules="[val => !!val || 'First name is required']"
                class="input-field"
              />
            </div>

            <div>
              <label class="text-small q-mb-xs block">Last Name</label>
              <q-input
                v-model="form.lastName"
                filled
                dark
                placeholder="Enter your last name"
                :rules="[val => !!val || 'Last name is required']"
                class="input-field"
              />
            </div>

            <div>
              <label class="text-small q-mb-xs block">Email</label>
              <q-input
                v-model="form.email"
                type="email"
                filled
                dark
                placeholder="Enter your email"
                :rules="[
                  val => !!val || 'Email is required',
                  val => /.+@.+\..+/.test(val) || 'Invalid email format'
                ]"
                class="input-field"
              />
            </div>

            <div>
              <label class="text-small q-mb-xs block">Password</label>
              <q-input
                v-model="form.password"
                :type="showPassword ? 'text' : 'password'"
                filled
                dark
                placeholder="Create a password"
                :rules="[
                  val => !!val || 'Password is required',
                  val => val.length >= 6 || 'Password must be at least 6 characters'
                ]"
                class="input-field"
              >
                <template v-slot:append>
                  <q-icon
                    :name="showPassword ? 'visibility' : 'visibility_off'"
                    class="cursor-pointer"
                    @click="showPassword = !showPassword"
                  />
                </template>
              </q-input>
            </div>

            <div class="q-mt-lg">
              <q-btn
                type="submit"
                label="Create Wallet"
                class="full-width btn-primary"
                size="lg"
                :loading="loading"
                no-caps
                unelevated
              />
            </div>
          </q-form>
        </q-card-section>
      </q-card>

      <!-- Success Modal -->
      <q-dialog v-model="showSuccess" persistent>
        <q-card class="gradient-card" style="min-width: 400px;">
          <q-card-section class="text-center">
            <q-icon name="check_circle" size="80px" color="white" class="q-mb-md" />
            <h3 class="text-white q-mb-md">Wallet Created Successfully!</h3>
            <p class="text-white q-mb-sm">Your wallet address:</p>
            <div class="glass-effect q-pa-md" style="border-radius: 12px; word-break: break-all;">
              <strong>{{ walletAddress }}</strong>
            </div>
            <p class="text-white q-mt-md text-small">Balance: {{ balance }} {{ currency }}</p>
          </q-card-section>
          <q-card-actions align="center">
            <q-btn
              label="Go to Dashboard"
              color="white"
              text-color="purple"
              unelevated
              no-caps
              size="md"
              @click="goToDashboard"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>
    </div>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { useQuasar } from 'quasar'
import { useRouter } from 'vue-router'

const $q = useQuasar()
const router = useRouter()

const form = ref({
  firstName: '',
  lastName: '',
  email: '',
  password: ''
})

const showPassword = ref(false)
const loading = ref(false)
const showSuccess = ref(false)
const walletAddress = ref('')
const balance = ref(0)
const currency = ref('CBDC')

const onSubmit = async () => {
  loading.value = true

  try {
    const response = await fetch('http://localhost:5005/api/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        email: form.value.email,
        password: form.value.password,
        firstName: form.value.firstName,
        lastName: form.value.lastName
      })
    })

    const data = await response.json()

    if (data.success) {
      walletAddress.value = data.user.wallet.walletAddress
      balance.value = data.user.wallet.balance
      currency.value = data.user.wallet.currency
      
      // Save for dashboard demo
      localStorage.setItem('cbdc_user', JSON.stringify(data.user))
      
      showSuccess.value = true
    } else {
      $q.notify({
        type: 'negative',
        message: data.message || 'Registration failed',
        position: 'top'
      })
    }
  } catch (error) {
    $q.notify({
      type: 'negative',
      message: 'Failed to connect to server. Please ensure the backend is running.',
      position: 'top'
    })
  } finally {
    loading.value = false
  }
}

const goToDashboard = () => {
  showSuccess.value = false
  router.push('/dashboard')
}
</script>

<style scoped lang="scss">
.register-container {
  padding: 40px 20px;
  max-width: 600px;
  width: 100%;
}

.block {
  display: block;
}
</style>
