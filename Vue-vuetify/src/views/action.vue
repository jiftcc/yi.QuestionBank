<template>
  <v-card>
    <v-data-table
      :headers="headers"
      :items="desserts"
      sort-by="calories"
      class="elevation-1"
      item-key="Id"
      show-select
      v-model="selected"
      :search="search"
    >
      <template v-slot:top>
        <!-- 搜索框 -->
        <v-toolbar flat>
          
          <v-spacer></v-spacer>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            label="搜索"
            single-line
            hide-details
            class="mx-4"
          ></v-text-field>



          <!-- 添加提示框 -->
          <v-dialog v-model="dialog" max-width="500px">
            <template v-slot:activator="{ on, attrs }">
              <v-btn color="primary" dark class="mb-2 mx-2" v-bind="attrs" v-on="on">
                添加新项
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">{{ formTitle }}</span>
              </v-card-title>

              <v-card-text>
                <v-container>
                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.action_name"
                        label="权限名"
                      ></v-text-field>
                    </v-col>
                                        <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.router"
                        label="路由"
                      ></v-text-field>
                    </v-col>
                                        <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.icon"
                        label="图标"
                      ></v-text-field>
                    </v-col>
                    <!-- <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.fat"
                        label="Fat (g)"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.carbs"
                        label="Carbs (g)"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.protein"
                        label="Protein (g)"
                      ></v-text-field>
                    </v-col> -->
                  </v-row>
                </v-container>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="close"> 取消 </v-btn>
                <v-btn color="blue darken-1" text @click="save"> 保存 </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>

              <v-btn color="red" dark class="mb-2" @click="deleteAllItem()">
                删除所选
              </v-btn>

          <!-- 删除提示框 -->
          <v-dialog v-model="dialogDeleteAll" max-width="500px">
            <v-card>
              <v-card-title class="headline"
                >你确定要删除所选记录吗？</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeDeleteAll"
                  >取消</v-btn
                >
                <v-btn color="blue darken-1" text @click="deleteAllItemConfirm"
                  >确定</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>




          <!-- 删除提示框 -->
          <v-dialog v-model="dialogDelete" max-width="500px">
            <v-card>
              <v-card-title class="headline"
                >你确定要删除此条记录吗？</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeDelete"
                  >取消</v-btn
                >
                <v-btn color="blue darken-1" text @click="deleteItemConfirm"
                  >确定</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>

      <!-- 表格中的删除和修改 -->
      <template v-slot:item.actions="{ item }">
        <v-icon small class="mr-2" @click="editItem(item)"> mdi-pencil </v-icon>
        <v-icon small @click="deleteItem(item)"> mdi-delete </v-icon>
      </template>

      <!-- 初始化 -->
      <template v-slot:no-data>
        <v-btn color="primary" @click="initialize"> 刷新 </v-btn>
      </template>
    </v-data-table>

    <!-- <template>
    <v-pagination
      v-model="page"
      :length="15"
      :total-visible="7"
    ></v-pagination>
          </template> -->
  </v-card>
</template>
<script>
import actionApi from "@/api/actionApi"
export default {
  data: () => ({
    page: 1,
    selected: [],
    search: "",
    dialog: false,
    dialogDelete: false,
    dialogDeleteAll: false,
    headers: [
      {
        text: "编号",
        align: "start",
        value: "Id",
      },
      { text: "权限名", value: "action_name", sortable: false },
       { text: "路由", value: "router", sortable: false },
        { text: "图标", value: "icon", sortable: false },
      { text: "操作", value: "actions", sortable: false },
      
    ],
    desserts: [],
    editedIndex: -1,
    editedItem: {
      action_name: "",
      router: "",
      icon:""
    },
    defaultItem: {
      action_name: "",
      router: "/my/",
      icon:"mdi-lock"
    },
  }),

  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "添加数据" : "编辑数据";
    },
  },

  watch: {
    dialog(val) {
      val || this.close();
    },
    dialogDelete(val) {
      val || this.closeDelete();
    },
  },

  created() {
    this.initialize();
  },

  methods: {
    initialize() {
      actionApi.getActions().then((resp) => {
        const response=resp.data
        this.desserts = response;
      });
    },
    // 添加和修改都打开同一个编辑框
    editItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    deleteItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteAllItem() {
      if (this.selected.length>0)
      {
      this.dialogDeleteAll = true;
      }
    },
    deleteItemConfirm() {
     actionApi.delAction(this.editedItem.Id).then(()=>this.initialize());;
      this.closeDelete();
    },
    deleteAllItemConfirm() {
      var Ids=[];
      this.selected.forEach(function(item){Ids.push(item.Id)});
      actionApi.delAllAction(Ids).then(()=>this.initialize());
    this.closeDeleteAll();
    },
    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },

    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    closeDeleteAll() {
      this.dialogDeleteAll = false;
    },
    save() {
      if (this.editedIndex > -1) {
      actionApi.updateAction(this.editedItem).then(()=>this.initialize());
      } else {
        actionApi.addAction(this.editedItem).then(()=>this.initialize());
      }
      this.close();
    },
  },
};
</script>