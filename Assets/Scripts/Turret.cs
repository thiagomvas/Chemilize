using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Turret : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] private TextMeshProUGUI tm;
    public TileData data;
    [SerializeField] private Transform target;
    [SerializeField] private Transform rotatePivot;
    [SerializeField] private float range;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float baseDelay;
    [SerializeField] private ElementSO ammoType;
    [SerializeField] private int AmmoContribution;
    [SerializeField] private Transform shootPoint;
    LineRenderer line;

    private float ammo;

    float dot;
    float nextShoot;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        InvokeRepeating("UpdateEnemy", 0.5f, 0.5f);
        ammo = data.variableIndex;
        tm.text = $"Ammo Type: {ammoType.name}\nAmount: {ammo}";
    }

    private void Update()
    {
        if (target != null)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(target.position - rotatePivot.position);
            rotatePivot.rotation = Quaternion.Slerp(rotatePivot.rotation, lookOnLook, Time.deltaTime * turnSpeed);
            dot = Vector3.Dot(rotatePivot.forward, (target.position - rotatePivot.position).normalized);
        }
        else dot = 0;

        if (dot > 0.85f && ReferencesManager.Instance.timer >= nextShoot) {
            nextShoot = ReferencesManager.Instance.timer + baseDelay;
            Shoot();
        }

    }

    private void Shoot()
    {
        if (data.variableIndex <= 0) return;
        data.variableIndex--;
        SaveData.Current.grids[data.gridIndex].tiles[data.index] = data;

        RaycastHit hit;
        Physics.Raycast(shootPoint.position, shootPoint.forward, out hit);
        line.positionCount = 2;
        line.SetPosition(0, shootPoint.position);
        line.SetPosition(1, hit.point);

        if (hit.transform.TryGetComponent<Enemy>(out Enemy e)) e.Damage(damage);
        StartCoroutine(LaserAnim());

        tm.text = $"Ammo Type: {ammoType.name}\nAmount: {SaveData.Current.grids[data.gridIndex].tiles[data.index].variableIndex}";
    }

    private void UpdateEnemy()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, range);
        foreach(Collider col in cols)
        {
            if(col.TryGetComponent<Enemy>(out Enemy e))
            {
                float dist1 = Vector3.Distance(e.transform.position, this.transform.position);
                float dist2 = target != null ? Vector3.Distance(target.position, this.transform.position) : 999999;
                if(dist1 < dist2) target = e.transform;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Element>(out Element ele))
        {
            if (ele.element = ammoType)
            {
                data.variableIndex += AmmoContribution;
                SaveData.Current.grids[data.gridIndex].tiles[data.index] = data;

                Destroy(ele.gameObject);
                tm.text = $"Ammo Type: {ammoType.name}\nAmount: {SaveData.Current.grids[data.gridIndex].tiles[data.index].variableIndex}";
            }
        }
    }

    IEnumerator LaserAnim()
    {
        float thickness = 1;
        float animFrames = 20;
        for(int i = 1; i < animFrames; i++)
        {
            thickness = 1/i;
            line.widthMultiplier = thickness/2;
            yield return new WaitForSeconds(baseDelay/animFrames);

        }        
    }
}
